using System;
using UnityEngine;

namespace Confront.Player
{
    public class SteepSlope : IState
    {
        public string AnimationName => "MidAir";

        public void Enter(PlayerController player)
        {
            var min = 6f;
            var max = 15f;
            var magnitude = Mathf.Clamp(player.MovementParameters.Velocity.magnitude, min, max);

            var groundSensorResult = player.Sensor.CalculateGroundState(player);
            var downhillDirection = Vector3.Cross(Vector3.Cross(Vector3.up, groundSensorResult.GroundNormal), groundSensorResult.GroundNormal).normalized;

            player.MovementParameters.Velocity = downhillDirection * magnitude;
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
            var sensorResult = player.Sensor.CalculateGroundState(player);
            var velocity = player.MovementParameters.Velocity;
            var groundSensorResult = player.Sensor.CalculateGroundState(player);

            var groundNormal = groundSensorResult.GroundNormal;
            var downhillDirection = Vector3.Cross(Vector3.Cross(Vector3.up, groundNormal), groundNormal).normalized;
            var acceleration = player.MovementParameters.SlopeAcceleration;

            var velocityMagnitude = velocity.magnitude;
            velocityMagnitude += acceleration * Time.deltaTime;
            player.MovementParameters.Velocity = downhillDirection * velocityMagnitude;
        }
    }
}