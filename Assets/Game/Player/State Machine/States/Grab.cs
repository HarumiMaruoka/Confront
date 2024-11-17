using Confront.Input;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Confront.Player
{
    public class Grab : IState
    {
        public string AnimationName => "Grab";

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
            player.Animator.SetBool("Climb", false);

            player.DirectionController.IsEnable = true;
            player.CharacterController.enabled = true;

            player.MovementParameters.GrabIntervalTimer = player.MovementParameters.GrabInterval;
        }

        private async UniTask GrabAsync(PlayerController player)
        {
            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var offset = new Vector3(player.MovementParameters.GrabPositionOffset.x * sign, player.MovementParameters.GrabPositionOffset.y);

            var startPoint = player.transform.position;
            var endPoint = player.MovementParameters.GrabbablePosition + offset;
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
            player.CharacterController.enabled = false;

            player.Animator.SetBool("Grab", false);
            player.Animator.SetBool("Climb", true);

            var animationCurve = player.MovementParameters.ClimbUpFlow;
            var startPoint = player.transform.position;
            var time = 0f;
            var index = 0;
            while (true)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;
                var value = animationCurve.Evaluate(time);
                player.transform.position = startPoint + Vector3.up * value;
                time += Time.deltaTime;
                if (time >= animationCurve.keys[index].time)
                {
                    index++;
                    if (index >= animationCurve.length) break;
                }
                await UniTask.Yield();
            }
            player.transform.position = startPoint + Vector3.up * animationCurve.keys[animationCurve.length - 1].value;
            await UniTask.Yield();

            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            animationCurve = player.MovementParameters.TraverseFlow;
            startPoint = player.transform.position;
            time = 0f;
            index = 0;
            while (true)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;
                var value = animationCurve.Evaluate(time);
                time += Time.deltaTime;
                player.transform.position = startPoint + Vector3.right * value * sign;
                if (time >= animationCurve.keys[index].time)
                {
                    index++;
                    if (index >= animationCurve.length) break;
                }
                await UniTask.Yield();
            }

            player.transform.position = startPoint + Vector3.right * animationCurve.keys[animationCurve.length - 1].value * sign;
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
    }
}