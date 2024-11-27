using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class Jump : IState
    {
        public string AnimationName => "MidAir";

        public void Enter(PlayerController player)
        {
            player.MovementParameters.Velocity.y = player.MovementParameters.JumpForce;
        }

        public void Execute(PlayerController player)
        {
            Move(player);
            StateTransition(player);
        }

        public void Exit(PlayerController player)
        {

        }

        private bool IsTurning(float inputDirection, float velocityX)
        {
            return (inputDirection > 0.1f && velocityX < -0.1f) || (inputDirection < -0.1f && velocityX > 0.1f);
        }

        private void Move(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);
            var inputX = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>().x;

            if (player.MovementParameters.Velocity.y > 0.0001f && sensorResult.IsAbove)
            {
                player.MovementParameters.Velocity.y = 0;
            }

            var acceleration = player.MovementParameters.InAirXAcceleration;
            var deceleration = player.MovementParameters.InAirXDeceleration;
            var maxSpeed = player.MovementParameters.InAirXMaxSpeed;
            var gravity = player.MovementParameters.Gravity;
            var isInputZero = Mathf.Abs(inputX) < 0.01f;
            var isTurning = IsTurning(inputX, player.MovementParameters.Velocity.x);

            if (isInputZero)
            {
                player.MovementParameters.Velocity.x = Mathf.MoveTowards(player.MovementParameters.Velocity.x, 0f, deceleration * Time.deltaTime);
            }
            else if (isTurning)
            {
                player.MovementParameters.Velocity.x = 0f;
            }
            else
            {
                var inputDirection = Mathf.Sign(inputX);
                player.MovementParameters.Velocity.x = Mathf.MoveTowards(player.MovementParameters.Velocity.x, maxSpeed * inputDirection, acceleration * Time.deltaTime);
            }

            player.MovementParameters.Velocity.y -= gravity * Time.deltaTime;
        }

        private void StateTransition(PlayerController player)
        {
            if (player.MovementParameters.Velocity.y > 0f) return;

            var sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
                case GroundType.Abyss:
                    player.StateMachine.ChangeState<Abyss>();
                    break;
                case GroundType.SteepSlope:
                    player.StateMachine.ChangeState<SteepSlope>();
                    break;
                case GroundType.InAir:
                    player.StateMachine.ChangeState<InAir>();
                    break;
                case GroundType.Ground:
                    player.StateMachine.ChangeState<Grounded>();
                    break;
            }
        }
    }
}