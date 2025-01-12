using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Idle")]
    public class Idle : TransitionableStateBase, IState
    {
        [Header("Duration")]
        [SerializeField]
        private float _minDuration = 3f;
        [SerializeField]
        private float _maxDuration = 5f;

        private float _duration = 0f;
        private float _elapsed = 0f;

        private void OnValidate()
        {
            if (_minDuration > _maxDuration)
            {
                _maxDuration = _minDuration;
            }
        }

        public string AnimationName => "StandIdle";

        public void Enter(LeviathanController owner)
        {
            _duration = UnityEngine.Random.Range(_minDuration, _maxDuration);
            _elapsed = 0f;
        }

        public void Execute(LeviathanController owner)
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= _duration)
            {
                TransitionToNextState(owner);
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}