using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public abstract class PlayerState00Base : IState
    {
        [AnimationParameter, SerializeField]
        protected string _animParameterName = default;
        public string AnimParameterName => _animParameterName;

        protected PlayerStateMachine _stateMachine = null;

        public void Init(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Update()
        {

        }
    }
}