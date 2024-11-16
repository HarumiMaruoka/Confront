using Confront.Input;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.Player
{
    public class Grab : IState
    {
        private CancellationTokenSource _cancellationTokenSource;

        public async void Enter(PlayerController player)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            player.DirectionController.IsEnable = false;
            player.CharacterController.enabled = false;
            player.MovementParameters.Velocity = Vector3.zero;

            await GrabAsync(player);
            var input = await LeftStickInputAsync(player);

            if (IsClimb(player, input))
            {
                await ClimbAsync(player);
            }
            StateTransition(player);
        }

        public void Update(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {
            _cancellationTokenSource.Cancel();

            player.DirectionController.IsEnable = true;
            player.CharacterController.enabled = true;

            player.MovementParameters.GrabIntervalTimer = player.MovementParameters.GrabInterval;
            TimerUpdateAsync(player);
        }

        private async UniTask GrabAsync(PlayerController player)
        {
            var rotation = player.DirectionController.CurrentRotation;
            var startPoint = player.transform.position;
            var endPoint = player.MovementParameters.GrabbablePosition + rotation * (Vector3)player.MovementParameters.GrabPositionOffset;
            var duration = player.MovementParameters.GrabDuration;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                player.transform.position = Vector3.Lerp(startPoint, endPoint, t / duration);
                await UniTask.Yield();
            }

            player.transform.position = endPoint;
        }

        private async UniTask<Vector2> LeftStickInputAsync(PlayerController player)
        {
            while (true)
            {
                if (player == null) return default;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return default;
                var input = PlayerInputHandler.PlayerInput.InGameInput.Movement.ReadValue<Vector2>();

                if (input.magnitude > 0.1f) return input;

                await UniTask.Yield();
            }
        }

        private async UniTask ClimbAsync(PlayerController player)
        {
            if (player == null) return;
            var startPoint = player.transform.position;
            var endPoint = player.transform.position + player.MovementParameters.ClimbHeight * Vector3.up;
            var duration = player.MovementParameters.CLimbDurationY;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                player.transform.position = Vector3.Lerp(startPoint, endPoint, t / duration);
                await UniTask.Yield();
            }

            player.transform.position = endPoint;
            await UniTask.Yield();

            var direction = player.DirectionController.CurrentRotation * Vector3.right;
            startPoint = player.transform.position;
            endPoint = player.transform.position + player.MovementParameters.ClimbedPositionDistance * direction;
            duration = player.MovementParameters.ClimbDurationX;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                player.transform.position = Vector3.Lerp(startPoint, endPoint, t / duration);
                await UniTask.Yield();
            }

            player.transform.position = endPoint;
            await UniTask.Yield();
        }

        private bool IsClimb(PlayerController player, Vector2 leftStick)
        {
            var direction = player.DirectionController.CurrentDirection;
            var inputAngle = Mathf.Atan2(leftStick.y, leftStick.x) * Mathf.Rad2Deg;
            if (direction == Direction.Right)
            {
                // 右向きの場合、スティックの入力角度が -45° から 135° の間にあれば降りる。
                if (inputAngle < -45 || inputAngle > 135)
                {
                    return false;
                }
                // それ以外は登る。
                return true;
            }
            else
            {
                // 左向きの場合、スティックの入力角度が -135° から 45° の間にあれば降りる。
                if (inputAngle > -135 && inputAngle < 45)
                {
                    return false;
                }
                // それ以外は登る。
                return true;
            }
        }

        private void StateTransition(PlayerController player)
        {
            if (player == null) return;
            var sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
                case GroundType.InAir:
                    player.StateMachine.ChangeState<InAir>();
                    break;
                case GroundType.SteepSlope:
                    player.StateMachine.ChangeState<SteepSlope>();
                    break;
                case GroundType.Abyss:
                    player.StateMachine.ChangeState<Abyss>();
                    break;
                case GroundType.Ground:
                    player.StateMachine.ChangeState<Grounded>();
                    break;
            }
        }

        public async void TimerUpdateAsync(PlayerController player)
        {
            while (player.MovementParameters.GrabIntervalTimer > 0)
            {
                if (player == null) return;
                player.MovementParameters.GrabIntervalTimer -= Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
}