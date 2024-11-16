using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class Grounded : IState
    {
        public string AnimationName => "Run";

        public void Enter(PlayerController player)
        {
            player.MovementParameters.Velocity.y = 0f;
        }

        public void Update(PlayerController player)
        {
            Move(player);
            StateTransition(player);
        }

        public void Exit(PlayerController player)
        {

        }

        private float CalculateDirection(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return 0;
            return Mathf.Sign(direction);
        }

        private bool IsTurning(float inputDirection, float velocityX)
        {
            return (inputDirection > 0.1f && velocityX < -0.1f) || (inputDirection < -0.1f && velocityX > 0.1f);
        }

        private void Move(PlayerController player)
        {
            // 入力に応じてx速度を更新する。
            var inputX = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>().x;
            var groundNormal = player.Sensor.Calculate(player).GroundNormal;

            var acceleration = player.MovementParameters.Acceleration;
            var deceleration = player.MovementParameters.Deceleration;
            var inputDirection = CalculateDirection(inputX);

            var isTurning = IsTurning(inputDirection, player.MovementParameters.Velocity.x);
            var velocitySign = Mathf.Sign(player.MovementParameters.Velocity.x);
            float velocityMagnitude = player.MovementParameters.Velocity.magnitude * velocitySign;
            var isInputZero = inputDirection == 0f;

            if (isInputZero)
            {
                velocityMagnitude = Mathf.Lerp(velocityMagnitude, 0f, deceleration * Time.deltaTime);
            }
            else if (isTurning)
            {
                velocityMagnitude = Mathf.Lerp(velocityMagnitude, 0f, player.MovementParameters.TurnDeceleration * Time.deltaTime);
            }
            else
            {
                velocityMagnitude = Mathf.Lerp(velocityMagnitude, player.MovementParameters.MaxSpeed * inputDirection, acceleration * Time.deltaTime);
            }

            player.MovementParameters.Velocity = Vector3.ProjectOnPlane(new Vector3(velocityMagnitude, 0f), groundNormal).normalized * Mathf.Abs(velocityMagnitude);
        }

        private void StateTransition(PlayerController player)
        {
            SensorResult sensorResult = player.Sensor.Calculate(player);

            switch (sensorResult.GroundType)
            {
                case GroundType.SteepSlope:
                    player.StateMachine.ChangeState<SteepSlope>();
                    break;
                case GroundType.Abyss:
                    player.StateMachine.ChangeState<Abyss>();
                    break;
                case GroundType.InAir:
                    player.StateMachine.ChangeState<InAir>();
                    break;
            }
        }
    }
}