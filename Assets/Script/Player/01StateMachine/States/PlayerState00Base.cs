using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public abstract class PlayerState00Base : IState
    {
        [AnimationParameter, SerializeField]
        protected string _enterAnimParameterName = default;
        public string AnimParameterName => _enterAnimParameterName;

        protected PlayerStateMachine _stateMachine = null;

        public virtual void Init(PlayerStateMachine stateMachine)
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