using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public abstract class PlayerState00Base : IState
    {
        [AnimationParameter, SerializeField]
        protected string _transitionAnimParameterName = default;
        public string AnimParameterName => _transitionAnimParameterName;

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