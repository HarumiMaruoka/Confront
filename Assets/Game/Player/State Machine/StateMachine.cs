﻿using System;
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

        public IState GetState<T>() where T : IState
        {
            var type = typeof(T);
            if (!_states.ContainsKey(type))
            {
                _states[type] = (IState)Activator.CreateInstance(type);
            }

            return _states[type];
        }

        public void ChangeState<T>() where T : IState
        {
            var newState = GetState<T>();

            if (CurrentState != null)
            {
                CurrentState.Exit(_player);
                _player.Animator.SetBool(CurrentState.AnimationName, false);
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            CurrentState.Enter(_player);
            _player.Animator.SetBool(CurrentState.AnimationName, true);
        }

        public void ChangeState(Type type)
        {
            if (!typeof(IState).IsAssignableFrom(type))
            {
                Debug.LogError($"{type} is not assignable from IState");
                return;
            }
            if (!_states.ContainsKey(type))
            {
                _states[type] = (IState)Activator.CreateInstance(type);
            }
            if (CurrentState != null)
            {
                CurrentState.Exit(_player);
                _player.Animator.SetBool(CurrentState.AnimationName, false);
            }

            PreviousState = CurrentState;
            CurrentState = _states[type];

            CurrentState.Enter(_player);
            _player.Animator.SetBool(CurrentState.AnimationName, true);
        }

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update(_player);
            }
        }
    }
}