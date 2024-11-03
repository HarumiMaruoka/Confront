using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player.States
{
    public class Idle : IPlayerState
    {
        public string AnimationStateName => "Idle";
        public bool IsMovementInputEnabled => true;

        public void Enter(PlayerController player)
        {

        }

        public void Update(PlayerController player)
        {
            // 移動の入力があればRunに遷移
            if (PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>() != Vector2.zero)
            {
                player.StateMachine.ChangeState<Run>();
            }
            // ジャンプの入力があればJumpに遷移
            else if (PlayerInputHandler.InGameInput.Jump.triggered)
            {
                player.StateMachine.ChangeState<Jump>();
            }
        }

        public void Exit(PlayerController player)
        {

        }
    }
}