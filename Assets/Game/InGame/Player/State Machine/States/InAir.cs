using Confront.Debugger;
using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class InAir : IState
    {
        public string AnimationName => "MidAir";

        private Vector2 _prevPosition;
        private Vector2 _nextPosition;

        public void Enter(PlayerController player)
        {
            _prevPosition = player.transform.position;
            _nextPosition = player.transform.position;
        }

        public void Execute(PlayerController player)
        {
            _prevPosition = _nextPosition;
            _nextPosition = player.transform.position;

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

        private float CalculateDirection(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return 0;
            return Mathf.Sign(direction);
        }

        private void Move(PlayerController player)
        {
            var sensorResult = player.Sensor.Calculate(player);
            var inputX = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>().x;

            // 進行方向にモノがあるか
            var playerDirection = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var velocityDirection = CalculateDirection(player.MovementParameters.Velocity.x);
            var hit = player.Sensor.FrontCheck(player, playerDirection);
            if (playerDirection == velocityDirection && hit)
            {
                inputX = 0f;
                player.MovementParameters.Velocity.x = 0f;
            }

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
                player.DirectionController.UpdateVelocity(player.MovementParameters.Velocity);
            }

            player.MovementParameters.Velocity.y -= gravity * Time.deltaTime;
            var fallSpeed = player.MovementParameters.Velocity.y;
            var maxFallSpeed = player.MovementParameters.InAirMaxFallSpeed;
            if (fallSpeed < maxFallSpeed) player.MovementParameters.Velocity.y = maxFallSpeed;

            player.HandlePlatformCollision();
        }

        private void StateTransition(PlayerController player)
        {
            // 攻撃入力があれば攻撃ステートに遷移する。
            if (PlayerInputHandler.InGameInput.AttackX.triggered && DebugParams.Instance.CanPlayerAttack)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.AirRootX);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }
            if (PlayerInputHandler.InGameInput.AttackY.triggered && DebugParams.Instance.CanPlayerAttack)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.AirRootY);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }

            // 掴みポイントがセンサーにヒットすれば掴みステートに遷移する。
            var sensorResult = player.Sensor.Calculate(player);

            var isGrabbable =
                player.MovementParameters.IsGrabbableTimerFinished &&
                sensorResult.GrabbablePoint != null &&
                player.DirectionController.CurrentDirection == sensorResult.GrabbablePoint.Direction;

            if (isGrabbable)
            {
                player.MovementParameters.GrabbablePosition = sensorResult.GrabbablePoint.transform.position;
                player.StateMachine.ChangeState<Grab>();
                return;
            }

            // 特に何もなければデフォルトステートに遷移する。
            this.TransitionToDefaultState(player);
        }
    }
}