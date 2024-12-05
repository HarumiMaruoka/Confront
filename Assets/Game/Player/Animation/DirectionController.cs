using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.Player
{
    [Serializable]
    public class DirectionController
    {
        private Transform _transform;
        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField]
        private Vector3 RightDirectionAngle = new Vector3(0, 0, 0);
        [SerializeField]
        private Vector3 LeftDirectionAngle = new Vector3(0, 180, 0);
        [SerializeField]
        private float RotationSpeed = 5f;

        public Direction CurrentDirection;
        public Quaternion CurrentRotation => CurrentDirection == Direction.Right ? Quaternion.Euler(RightDirectionAngle) : Quaternion.Euler(LeftDirectionAngle);

        public event Action<Direction> OnDirectionChanged;

        public bool IsEnable = true;

        public void Initialize(Transform transform)
        {
            _transform = transform;
        }

        public void UpdateVelocity(Vector2 velocity)
        {
            if (!IsEnable) return;

            if (Mathf.Abs(velocity.x) <= 0.001f) return;

            if (velocity.x > 0 && CurrentDirection != Direction.Right)
            {
                ChangeDirection(Direction.Right);
            }
            else if (velocity.x < 0 && CurrentDirection != Direction.Left)
            {
                ChangeDirection(Direction.Left);
            }
        }

        private void ChangeDirection(Direction direction)
        {
            CurrentDirection = direction;
            RotateToDirection(direction).Forget();
            OnDirectionChanged?.Invoke(direction);
        }

        private async UniTaskVoid RotateToDirection(Direction direction)
        {
            var targetRotation = direction == Direction.Right ? RightDirectionAngle : LeftDirectionAngle;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            while (true)
            {
                if (!_transform) break;
                _transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.Euler(targetRotation), RotationSpeed * Time.deltaTime);
                if (Quaternion.Angle(_transform.rotation, Quaternion.Euler(targetRotation)) < 0.1f)
                {
                    _transform.rotation = Quaternion.Euler(targetRotation);
                    break;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }
        }
    }

    public enum Direction
    {
        Right,
        Left,
    }
}