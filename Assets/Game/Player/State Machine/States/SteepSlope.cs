using System;
using UnityEngine;

namespace Confront.Player
{
    public class SteepSlope : IState
    {
        public void Enter(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {

        }

        public void Update(PlayerController player)
        {
            Move(player);
            StateTransition(player);
        }

        private void Move(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);
            var velocity = player.MovementParameters.Velocity;
            var groundSensorResult = player.Sensor.Calculate(player);

            var groundNormal = groundSensorResult.GroundNormal;
            var downhillDirection = Vector3.Cross(Vector3.Cross(Vector3.up, groundNormal), groundNormal).normalized;
            var acceleration = player.MovementParameters.SlopeAcceleration;

            var velocityMagnitude = velocity.magnitude;
            velocityMagnitude += acceleration * Time.deltaTime;
            player.MovementParameters.Velocity = downhillDirection * velocityMagnitude;
        }

        private void StateTransition(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
                case GroundType.InAir:
                    player.StateMachine.ChangeState<InAir>();
                    break;
                case GroundType.Abyss:
                    player.StateMachine.ChangeState<Abyss>();
                    break;
                case GroundType.Ground:
                    player.StateMachine.ChangeState<Grounded>();
                    break;
            }
        }
    }
}