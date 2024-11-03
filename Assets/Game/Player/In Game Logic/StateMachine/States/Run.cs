using System;
using UnityEngine;

namespace Confront.Player.States
{
    public class Run : IPlayerState
    {
        public string AnimationStateName => "Run";

        public bool IsMovementInputEnabled => true;

        public void Enter(PlayerController player)
        {

        }

        public void Update(PlayerController player)
        {
            var maxSpeed = player.MovementSystem.MaxSpeed;
            var currentSpeed = player.MovementSystem.Velocity.magnitude;
            var speedRatio = currentSpeed / maxSpeed;
            player.Animator.SetFloat("RunSpeed", speedRatio);

            if (currentSpeed < 0.01) // ほぼ停止していれば Idleステートに遷移する。
            {
                player.StateMachine.ChangeState<Idle>();
            }
        }

        public void Exit(PlayerController player)
        {

        }
    }
}