using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class Abyss : IState
    {
        public string AnimationName => "MidAir";

        public void Enter(PlayerController player)
        {

        }

        public void Execute(PlayerController player)
        {
            Move(player);
            this.TransitionToDefaultState(player);
        }

        public void Exit(PlayerController player)
        {

        }

        public static void Move(PlayerController player)
        {
            var parameters = player.MovementParameters;
            var gravity = parameters.Gravity;
            player.MovementParameters.Velocity.y -= gravity * Time.deltaTime;

            var groundSensorResult = player.Sensor.CalculateGroundState(player);
            if (!groundSensorResult.IsGrounded) return;

            var velocity = player.MovementParameters.Velocity;

            var fallDirection = Mathf.Sign(groundSensorResult.GroundNormal.x);
            var acceleration = parameters.AbyssAcceleration;

            if (Mathf.Abs(velocity.x) < player.MovementParameters.AbyssXMinSpeed && groundSensorResult.GroundNormal.x != 0f)
            {
                // 最低速度を保証する。
                player.MovementParameters.Velocity.x = player.MovementParameters.AbyssXMinSpeed * fallDirection;
            }

            var maxSpeed = player.MovementParameters.InAirXMaxSpeed;
            acceleration = player.MovementParameters.InAirXAcceleration;
            player.MovementParameters.Velocity.x = Mathf.MoveTowards(player.MovementParameters.Velocity.x, maxSpeed * fallDirection, acceleration * Time.deltaTime);
        }
    }
}
