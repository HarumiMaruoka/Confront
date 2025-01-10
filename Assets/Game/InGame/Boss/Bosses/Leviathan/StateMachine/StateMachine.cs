using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [Serializable]
    public class StateMachine
    {
        [SerializeField]
        private Idle _idle;
        [SerializeField]
        private Walk _walk;
        [SerializeField]
        private Stunned _stunned;
        [SerializeField]
        private GetHit1 _getHit1;
        [SerializeField]
        private GetHit2 _getHit2;
        [SerializeField]
        private Die _die;
        [SerializeField]
        private Attack1 _attack;
        [SerializeField]
        private Attack2 _attack2;
        [SerializeField]
        private AttackHard _attackHard;
        [SerializeField]
        private AttackSpecial _attackSpecial;
        [SerializeField]
        private Roar _roar;
        [SerializeField]
        private Block _block;

        private Dictionary<Type, IState> _states;

        private void InitializeStates()
        {
            _states = new Dictionary<Type, IState>()
            {
                { typeof(Idle), _idle},
                { typeof(Walk), _walk},
                { typeof(Stunned), _stunned},
                { typeof(GetHit1), _getHit1},
                { typeof(GetHit2), _getHit2},
                { typeof(Die), _die},
                { typeof(Attack1), _attack},
                { typeof(Attack2), _attack2},
                { typeof(AttackHard), _attackHard},
                { typeof(AttackSpecial), _attackSpecial},
                { typeof(Roar), _roar},
                { typeof(Block), _block },
            };
        }

        private LeviathanController _owner;
        private IState _currentState;

        public void Initialize(LeviathanController leviathan)
        {
            _owner = leviathan;
            InitializeStates();
            _currentState = _idle;
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
        }

        public void Update()
        {
            _currentState.Execute(_owner);
        }
    }
}