using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Rotate")]
    public class Rotate : TransitionableStateBase, IState
    {
        [SerializeField]
        private Vector3 _rightRotation = new Vector3(0f, 90f, 0f);
        [SerializeField]
        private Vector3 _leftRotation = new Vector3(0f, -90f, 0f);

        [SerializeField]
        private float _duration = 1f;

        private float _timer = 0f;

        private float _beginRotationY = 0f;
        private float _targetRotationY = 0f;

        public string AnimationName => "WalkCycle";

        public void Enter(LeviathanController owner)
        {
            var player = PlayerController.Instance;
            var distanceToPlayer = owner.transform.position.x - player.transform.position.x > 0f ? Direction.Left : Direction.Right;
            if (owner.Direction == distanceToPlayer)
            {
                TransitionToNextState(owner);
                return;
            }

            _timer = 0f;
            _beginRotationY = owner.Direction == Direction.Right ? _rightRotation.y : _leftRotation.y;
            _targetRotationY = owner.Direction == Direction.Right ? _leftRotation.y : _rightRotation.y;
            owner.Direction = owner.Direction == Direction.Right ? Direction.Left : Direction.Right;
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;

            if (_timer >= _duration)
            {
                TransitionToNextState(owner);
                owner.transform.rotation = Quaternion.Euler(0f, _targetRotationY, 0f);
                return;
            }

            var rotationY = Mathf.Lerp(_beginRotationY, _targetRotationY, _timer / _duration);
            owner.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}