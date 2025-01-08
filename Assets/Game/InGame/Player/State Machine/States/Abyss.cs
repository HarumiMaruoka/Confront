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
            var velocity = player.MovementParameters.Velocity;
            var settings = player.MovementParameters;
            var groundSensorResult = player.Sensor.CalculateGroundState(player);

            var fallDirection = Mathf.Sign(groundSensorResult.GroundNormal.x);
            var velocityDirection = Mathf.Sign(velocity.x);
            var acceleration = settings.AbyssAcceleration;

            if (Mathf.Abs(velocity.x) < player.MovementParameters.AbyssXMinSpeed)
            {
                // 最低速度を保証する。
                fallDirection = player.MovementParameters.AbyssXMinSpeed * fallDirection;
            }

            player.MovementParameters.Velocity += new Vector2(fallDirection, -1f) * acceleration * Time.deltaTime;
        }
    }
}
