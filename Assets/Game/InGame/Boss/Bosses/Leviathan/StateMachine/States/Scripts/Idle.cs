using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Idle")]
    public class Idle : ScriptableObject, IState
    {
        [SerializeField]
        private float _minIdleTime = 3f;
        [SerializeField]
        private float _maxIdleTime = 5f;

        private float _idleTime = 0f;
        private float _elapsedTime = 0f;

        public string AnimationName => "StandIdle";

        public void Enter(LeviathanController owner)
        {
            _idleTime = UnityEngine.Random.Range(_minIdleTime, _maxIdleTime);
            _elapsedTime = 0f;
        }

        public void Execute(LeviathanController owner)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _idleTime)
            {
                // owner.ChangeState(owner.Walk);
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}