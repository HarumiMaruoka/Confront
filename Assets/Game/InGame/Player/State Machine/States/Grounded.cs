﻿using Confront.Input;
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

        public void Execute(PlayerController player)
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
            var leftStick = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>();
            var jumpInput = PlayerInputHandler.InGameInput.Jump.triggered;
            if (leftStick.sqrMagnitude > 0.1f)
            {
                var angle = Vector2.Angle(leftStick, Vector2.down);
                if (jumpInput && angle < 45f)
                {
                    var disableTime = player.MovementParameters.PassThroughPlatformDisableTime;
                    player.MovementParameters.PassThroughPlatformDisableTimer = disableTime;
                }
            }

            // 入力に応じてx速度を更新する。
            var inputX = leftStick.x;
            var groundSensorResult = player.Sensor.Calculate(player);
            var groundNormal = groundSensorResult.GroundNormal;

            var acceleration = player.MovementParameters.Acceleration;
            var deceleration = player.MovementParameters.Deceleration;
            var inputDirection = CalculateDirection(inputX);

            var isTurning = IsTurning(inputDirection, player.MovementParameters.Velocity.x);
            var velocitySign = Mathf.Sign(player.MovementParameters.Velocity.x);
            float velocityMagnitude = player.MovementParameters.Velocity.magnitude * velocitySign;
            var isInputZero = inputDirection == 0f;

            var slopeAngle = Vector3.Angle(Vector3.up, groundNormal);
            var groundNormalSign = CalculateDirection(groundNormal.x);
            var IsFacingSteepSlope = (groundNormalSign == 1 && inputDirection == -1) || (groundNormalSign == -1 && inputDirection == 1);

            if (slopeAngle >= player.CharacterController.slopeLimit && IsFacingSteepSlope)
            {
                velocityMagnitude = Mathf.Lerp(velocityMagnitude, 0f, deceleration * Time.deltaTime);
            }
            else if (isInputZero)
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

            // 進行方向にモノがあるか
            var playerDirection = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var velocityDirection = CalculateDirection(velocityMagnitude);
            var hit = player.Sensor.FrontCheck(player, playerDirection);
            if (playerDirection == velocityDirection && hit)
            {
                velocityMagnitude = 0f;
            }

            player.MovementParameters.Velocity = Vector3.ProjectOnPlane(new Vector3(velocityMagnitude, 0f), groundNormal).normalized * Mathf.Abs(velocityMagnitude);
        }

        private void StateTransition(PlayerController player)
        {
            // 入力に応じて攻撃状態に遷移する。
            if (PlayerInputHandler.InGameInput.AttackX.triggered)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.GroundRootX);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }
            if (PlayerInputHandler.InGameInput.AttackY.triggered)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.GroundRootY);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }

            // 入力に応じて回避状態に遷移する。
            if (PlayerInputHandler.InGameInput.Dodge.triggered)
            {
                player.StateMachine.ChangeState<GroundDodge>();
                return;
            }

            // 上り坂に向かっている場合
            var playerVelocity = player.MovementParameters.Velocity;
            var groundSensorResult = player.Sensor.Calculate(player);
            var groundNormal = groundSensorResult.GroundNormal;
            var isRightSlope = groundNormal.x < 0;

            if (groundSensorResult.GroundType == GroundType.SteepSlope &&
               (isRightSlope && playerVelocity.x > 0 ||
               !isRightSlope && playerVelocity.x < 0))
            {
                return;
            }

            this.TransitionToDefaultState(player);
        }
    }
}