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

        public void Enter(PlayerController player)
        {
            // 各種パラメータを初期化する。
            _elapsed = 0f;
            _initialSpeed = player.MovementParameters.Velocity.magnitude;
            if (EnemyLayer == 0) EnemyLayer = LayerMask.NameToLayer(EnemyLayerName);

            // 敵との当たり判定を遮断する。
            Physics.IgnoreLayerCollision(player.gameObject.layer, EnemyLayer, true);
        }

        public void Execute(PlayerController player)
        {
            var maxSpeed = player.MovementParameters.DodgeMaxSpeed;
            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var groundNormal = player.Sensor.Calculate(player).GroundNormal;

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

            var velocity = new Vector3(_speed * sign, 0f);
            player.MovementParameters.Velocity = Vector3.ProjectOnPlane(velocity, groundNormal).normalized * Mathf.Abs(_speed);
        }

        public void Exit(PlayerController player)
        {
            // 敵との当たり判定を元に戻す。
            Physics.IgnoreLayerCollision(player.gameObject.layer, EnemyLayer, false);
        }
    }
}