using System;
using UnityEngine;

namespace Confront.Player
{
    public class Abyss : IState
    {
        public void Enter(PlayerController player)
        {

        }

        public void Update(PlayerController player)
        {
            Move(player);
            StateTransition(player);
        }

        public void Exit(PlayerController player)
        {

        }

        private void Move(PlayerController player)
        {
            var velocity = player.MovementParameters.Velocity;
            var settings = player.MovementParameters;
            var groundSensorResult = player.Sensor.Calculate(player);

            var fallDirection = Mathf.Sign(groundSensorResult.GroundNormal.x);
            var velocityDirection = Mathf.Sign(velocity.x);
            var acceleration = settings.AbyssAcceleration;

            if (Mathf.Abs(velocity.x) < player.MovementParameters.AbyssXMinSpeed)
            {
                // Å’á‘¬“x‚ð•ÛØ‚·‚éB
                fallDirection = player.MovementParameters.AbyssXMinSpeed * fallDirection;
            }

            player.MovementParameters.Velocity += new Vector2(fallDirection, -1f) * acceleration * Time.deltaTime;
        }

        private void StateTransition(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
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
