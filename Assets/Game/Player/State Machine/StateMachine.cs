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

        public void ChangeState<T>() where T : IState
        {
            var newState = GetState(typeof(T));
            ChangeState(newState);
        }

        public void ChangeState(Type type)
        {
            var newState = GetState(type);
            ChangeState(newState);
        }

        public void ChangeState(IState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_player);
                if (!string.IsNullOrEmpty(CurrentState.AnimationName))
                    _player.Animator.SetBool(CurrentState.AnimationName, false);
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            CurrentState.Enter(_player);
            if (!string.IsNullOrEmpty(CurrentState.AnimationName))
                _player.Animator.SetBool(CurrentState.AnimationName, true);

            // Debug.Log($"New State: {CurrentState.GetType().Name}");
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute(_player);
            }
        }
    }
}