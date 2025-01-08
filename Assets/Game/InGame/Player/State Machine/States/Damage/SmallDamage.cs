using System;
using UnityEngine;

namespace Confront.Player
{
    // 少しノックバックするダメージ状態
    public class SmallDamage : IState
    {
        private float _airTimer = 0f;
        private float _exitTimer = 0f;
        private PlayerController _player;

        private float _airDuration = 0.1f;

        public string AnimationName => "SmallDamage";

        public Vector2 DamageDirection
        {
            set
            {
                if (!_player)
                {
                    Debug.LogError("Player is null");
                    return;
                }
                // ダメージ方向を設定
                _player.MovementParameters.Velocity = value;
            }
        }

        public void Enter(PlayerController player)
        {
            _player = player;
            _exitTimer = 0f;
        }

        public void Execute(PlayerController player)
        {
            var duration = player.MovementParameters.SmallDamageDuration;
            var horizontalDecelerationRate = player.MovementParameters.SmallDamageHorizontalDecelerationRate;

            _exitTimer += Time.deltaTime;
            if (_exitTimer >= duration)
            {
                this.TransitionToDefaultState(player);
                return;
            }

            if (_airDuration > _airTimer || player.MovementParameters.Velocity.y >= 0f)
            {
                _airTimer += Time.deltaTime;
                InAir.Move(player, true);
                return;
            }

            var groundSensorResult = player.Sensor.CalculateGroundState(player);

            switch (groundSensorResult.GroundType)
            {
                case GroundType.Ground: Grounded.Move(player, false, false); break;
                case GroundType.Abyss: Abyss.Move(player); break;
                case GroundType.SteepSlope: SteepSlope.Move(player); break;
                case GroundType.InAir: InAir.Move(player, false); break;
            }
        }

        public void Exit(PlayerController player)
        {

        }
    }
}