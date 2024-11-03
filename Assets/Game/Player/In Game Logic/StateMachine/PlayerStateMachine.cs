using Confront.Player.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player
{
    public class PlayerStateMachine
    {
        public PlayerStateMachine(PlayerController playerController)
        {
            _playerController = playerController;

            _stateMap = new Dictionary<Type, IPlayerState>
            {
                { typeof(Idle), new Idle() },
                { typeof(Run), new Run() },
                { typeof(Jump), new Jump() },
                { typeof(MidAir), new MidAir() },
                { typeof(Land), new Land() },
                { typeof(SmallDamage), new SmallDamage() },
                { typeof(BigDamage), new BigDamage() },
                { typeof(StandUp), new StandUp() },
                { typeof(Dead), new Dead() },
                { typeof(Revive), new Revive() },
            };
        }

        private PlayerController _playerController;
        private Dictionary<Type, IPlayerState> _stateMap;
        private IPlayerState _currentState;

        public void ChangeState<T>()
        {
            var old = _currentState;

            _currentState?.Exit(_playerController);
            _currentState = _stateMap[typeof(T)];

            OnStateChanged(_currentState, old);

            _currentState.Enter(_playerController);
        }

        public void Update()
        {
            _currentState?.Update(_playerController);
        }

        private void OnStateChanged(IPlayerState newState, IPlayerState oldState)
        {
            if (oldState != null)
            {
                _playerController.Animator.SetBool(oldState.AnimationStateName, false);
            }

            if (newState != null)
            {
                _playerController.Animator.SetBool(newState.AnimationStateName, true);

                if(newState.IsMovementInputEnabled) Confront.Input.PlayerInputHandler.InGameInput.Movement.Enable();
                else Confront.Input.PlayerInputHandler.InGameInput.Movement.Disable();
            }
        }
    }
}