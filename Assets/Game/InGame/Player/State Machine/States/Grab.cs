using Confront.Input;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                this.TransitionToDefaultState(player);
            }
        }

        public void Execute(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {
            _cancellationTokenSource.Cancel();

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

            player.Animator.CrossFade("Climb", 0.2f);

            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;

            // 上昇と水平方向のアニメーションカーブ
            var climbCurve = player.MovementParameters.ClimbUpFlow;
            var traverseCurve = player.MovementParameters.TraverseFlow;

            var maxTime = Mathf.Max(climbCurve.keys[climbCurve.length - 1].time, traverseCurve.keys[traverseCurve.length - 1].time);
            var threshold = player.MovementParameters.ClimbToRunThreshold;
            var previousTime = 0f;
            float time = 0f;
            if (player == null) return;
            Vector3 startPosition = player.transform.position;

            while (time < maxTime)
            {
                if (player == null) return;
                if (_cancellationTokenSource.Token.IsCancellationRequested) return;

                float verticalValue = climbCurve.Evaluate(time);
                float horizontalValue = traverseCurve.Evaluate(time);

                player.transform.position = startPosition + Vector3.up * verticalValue + Vector3.right * horizontalValue * sign;

                if (previousTime < maxTime - threshold && time >= maxTime - threshold)
                {
                    player.Animator.CrossFade("Run", 0.2f);
                }

                previousTime = time;
                time += Time.deltaTime;
                await UniTask.Yield();
            }

            // 最終位置を設定
            float finalVerticalValue = climbCurve.Evaluate(climbCurve.keys[climbCurve.length - 1].time);
            float finalHorizontalValue = traverseCurve.Evaluate(traverseCurve.keys[traverseCurve.length - 1].time);
            player.transform.position = startPosition + Vector3.up * finalVerticalValue + Vector3.right * finalHorizontalValue * sign;

            await UniTask.Yield();
        }


        private bool IsClimb(PlayerController player, Vector2 leftStick)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested) return false;

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
    }
}