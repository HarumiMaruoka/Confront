using Confront.Debugger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player
{
    public class StateMachine
    {
        private readonly PlayerController _player;

        private readonly Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }

        public StateMachine(PlayerController player)
        {
            _player = player;
        }

        public IState GetState(Type type)
        {
            if (!_states.ContainsKey(type))
            {
                _states[type] = (IState)Activator.CreateInstance(type);
            }
            return _states[type];
        }

        public T ChangeState<T>() where T : class, IState
        {
            var newState = GetState(typeof(T));
            return ChangeState(newState) as T;
        }

        public IState ChangeState(Type type)
        {
            var newState = GetState(type);
            return ChangeState(newState);
        }

        public IState ChangeState(IState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_player);
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            CurrentState.Enter(_player);
            if (!string.IsNullOrEmpty(CurrentState.AnimationName))
                _player.Animator.CrossFade(CurrentState.AnimationName, 0.1f);

            if (DebugParams.Instance.StateTransitionLogging)
                Debug.Log($"Transitioning from {PreviousState} to {CurrentState}");

            return CurrentState;
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute(_player);
            }
        }

        public void LateUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.LateExecute(_player);
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.FixedExecute(_player);
            }
        }
    }
}