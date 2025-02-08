using Confront.Debugger;
using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class InAir : IState
    {
        public string AnimationName => "MidAir";

        public void Enter(PlayerController player)
        {

        }

        public void Execute(PlayerController player)
        {
            Move(player);
            StateTransition(player);
        }

        public void Exit(PlayerController player)
        {

        }

        private static bool IsTurning(float inputDirection, float velocityX)
        {
            return (inputDirection > 0.1f && velocityX < -0.1f) || (inputDirection < -0.1f && velocityX > 0.1f);
        }

        private static float CalculateDirection(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return 0;
            return Mathf.Sign(direction);
        }

        public static void Move(PlayerController player, bool isInputReceived = true)
        {
            var inputX = 0f;
            if (isInputReceived) inputX = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>().x;
            Move(player, inputX);
        }

        public static void Move(PlayerController player, float inputX)
        {
            // 進行方向にモノがあるか
            var playerDirection = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var velocityDirection = CalculateDirection(player.MovementParameters.Velocity.x);
            var hit = !player.Sensor.IsOverlappingWithEnemy(player) && player.Sensor.FrontCheck(player, playerDirection);
            if (playerDirection == velocityDirection && hit)
            {
                inputX = 0f;
                player.MovementParameters.Velocity.x = 0f;
            }

            // 上昇中に頭上に物体があれば 縦の速度を0にする。
            if (player.MovementParameters.Velocity.y > 0.0001f && player.Sensor.IsAbove(player))
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
                player.DirectionController.UpdateDirection(player.MovementParameters.Velocity);
            }

            player.MovementParameters.Velocity.y -= gravity * Time.deltaTime;
            var fallSpeed = player.MovementParameters.Velocity.y;
            var maxFallSpeed = player.MovementParameters.InAirMaxFallSpeed;
            if (fallSpeed < maxFallSpeed) player.MovementParameters.Velocity.y = maxFallSpeed;
        }

        private void StateTransition(PlayerController player)
        {
            // 攻撃入力があれば攻撃ステートに遷移する。
            if (PlayerInputHandler.InGameInput.AttackX.triggered &&
                player.MovementParameters.IsAttackIntervalTimerFinished &&
                DebugParams.Instance.CanPlayerAttack)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.AirRootX);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }
            if (PlayerInputHandler.InGameInput.AttackY.triggered &&
                player.MovementParameters.IsAttackIntervalTimerFinished &&
                DebugParams.Instance.CanPlayerAttack)
            {
                var attackStateMachine = player.AttackStateMachine;
                attackStateMachine.Initialize(player.AttackComboTree, Combo.ComboTree.NodeType.AirRootY);
                player.StateMachine.ChangeState(attackStateMachine);
                return;
            }

            // 掴みポイントがセンサーにヒットすれば掴みステートに遷移する。
            var grabbablePoint = player.Sensor.GetGrabbablePoint(player);

            var isGrabbable =
                player.MovementParameters.IsGrabbableTimerFinished &&
                grabbablePoint != null &&
                player.DirectionController.CurrentDirection == grabbablePoint.Direction;

            if (isGrabbable)
            {
                player.MovementParameters.GrabbablePosition = grabbablePoint.transform.position;
                player.StateMachine.ChangeState<Grab>();
                return;
            }

            // 特に何もなければデフォルトステートに遷移する。
            this.TransitionToDefaultState(player);
        }
    }
}