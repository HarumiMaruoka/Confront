using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class GroundDodge : IState
    {
        public string AnimationName => "GroundDodge";

        private static readonly string EnemyLayerName = "Enemy";
        private LayerMask EnemyLayer = 0;
        private float _elapsed = 0f;

        private float _initialSpeed;
        private float _speed;
        private float _sign;

        public void Enter(PlayerController player)
        {
            // 各種パラメータを初期化する。
            _elapsed = 0f;
            _initialSpeed = player.MovementParameters.Velocity.magnitude;
            if (EnemyLayer == 0) EnemyLayer = LayerMask.NameToLayer(EnemyLayerName);

            var inputX = PlayerInputHandler.InGameInput.Movement.ReadValue<Vector2>().x;
            if (Mathf.Abs(inputX) < 0.1f)
            {
                _sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            }
            else
            {
                _sign = Mathf.Sign(inputX);
            }

            // 敵との当たり判定を遮断する。
            Physics.IgnoreLayerCollision(player.gameObject.layer, EnemyLayer, true);
        }

        public void Execute(PlayerController player)
        {
            var maxSpeed = player.MovementParameters.DodgeMaxSpeed;
            var groundSensorResult = player.Sensor.CalculateGroundState(player);

            var accelerationDuration = player.MovementParameters.DodgeAccelerationDuration;
            var accelerationTime = player.MovementParameters.DodgeAccelerationDuration;

            var decelerationDuration = player.MovementParameters.DodgeDecelerationDuration;
            var decelerationTime = accelerationTime + player.MovementParameters.DodgeDecelerationDuration;

            _elapsed += Time.deltaTime;

            if (_elapsed < accelerationTime)
            {
                // 加速フェーズ
                _speed = Mathf.Lerp(_initialSpeed, maxSpeed, _elapsed / accelerationDuration);
            }
            else if (_elapsed < decelerationTime)
            {
                // 減速フェーズ
                _speed = Mathf.Lerp(maxSpeed, 0f, (_elapsed - accelerationTime) / decelerationDuration);
            }
            else
            {
                this.TransitionToDefaultState(player);
            }

            var groundNormal = groundSensorResult.GroundNormal;
            var velocity = new Vector3(_speed * _sign, 0f);

            if (groundSensorResult.GroundType == GroundType.SteepSlope)
            {
                _speed = 0f;
            }
            else if (groundSensorResult.GroundType == GroundType.Ground)
            {
                player.MovementParameters.Velocity = Vector3.ProjectOnPlane(velocity, groundNormal).normalized * Mathf.Abs(_speed);
            }
            else
            {
                var velocityY = player.MovementParameters.Velocity.y;
                var gravity = player.MovementParameters.Gravity;
                player.MovementParameters.Velocity = new Vector3(velocity.x, velocityY - gravity * Time.deltaTime);
            }
        }

        public void Exit(PlayerController player)
        {
            // 敵との当たり判定を元に戻す。
            Physics.IgnoreLayerCollision(player.gameObject.layer, EnemyLayer, false);
        }
    }
}