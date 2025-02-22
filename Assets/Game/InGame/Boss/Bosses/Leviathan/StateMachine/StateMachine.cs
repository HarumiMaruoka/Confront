﻿using NexEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [Serializable]
    public class StateMachine
    {
        [Expandable] public Idle Idle;
        [Expandable] public Walk Walk;
        [Expandable] public Rotate Rotate;
        [Expandable] public Stunned Stunned;
        [Expandable] public GetHit1 GetHit1;
        [Expandable] public GetHit2 GetHit2;
        [Expandable] public Die Die;
        [Expandable] public Attack1 Attack1;
        [Expandable] public Attack2 Attack2;
        [Expandable] public AttackHard AttackHard;
        [Expandable] public AttackSpecial AttackSpecial;
        [Expandable] public Roar Roar;
        [Expandable] public Block Block;

        private Dictionary<Type, IState> _states;

        private void InitializeStates()
        {
            _states = new Dictionary<Type, IState>()
            {
                { typeof(Idle), Idle},
                { typeof(Walk), Walk},
                { typeof(Rotate), Rotate},
                { typeof(Stunned), Stunned},
                { typeof(GetHit1), GetHit1},
                { typeof(GetHit2), GetHit2},
                { typeof(Die), Die},
                { typeof(Attack1), Attack1},
                { typeof(Attack2), Attack2},
                { typeof(AttackHard), AttackHard},
                { typeof(AttackSpecial), AttackSpecial},
                { typeof(Roar), Roar},
                { typeof(Block), Block },
            };

            foreach (var state in _states.Values)
            {
                state.Initialize();
            }
        }

        private LeviathanController _owner;
        private IState _currentState;

        public IState CurrentState => _currentState;

        public void Initialize(LeviathanController leviathan)
        {
            _owner = leviathan;
            InitializeStates();
            _currentState = Idle;
            _currentState.Enter(leviathan);
        }

        public void ChangeState<T>()
        {
            ChangeState(typeof(T));
        }

        public void ChangeState(Type stateType)
        {
            if (_currentState != null)
            {
                _currentState.Exit(_owner);
            }
            _currentState = _states[stateType];
            _currentState.Enter(_owner);

            _owner.Animator.CrossFade(_currentState.AnimationName, _currentState.AnimationCrossFadeTime);
        }

        public void ChangeState(IState state)
        {
            if (_currentState != null)
            {
                _currentState.Exit(_owner);
            }
            _currentState = state;
            _currentState.Enter(_owner);

            _owner.Animator.CrossFade(_currentState.AnimationName, _currentState.AnimationCrossFadeTime);
        }

        public void Update()
        {
            _currentState.Execute(_owner);
        }
    }
}