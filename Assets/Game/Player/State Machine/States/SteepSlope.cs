using System;
using UnityEngine;

namespace Confront.Player
{
    public class SteepSlope : IState
    {
        public string AnimationName => "MidAir";

        public void Enter(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {

        }

        public void Execute(PlayerController player)
        {
            Move(player);
            this.TransitionToDefaultState(player);
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
    }
}