using System;
using UnityEngine;

namespace Confront.Player
{
    // 大きくノックバックするダメージ状態
    public class BigDamage : IState
    {
        private float _airTimer = 0f;
        private float _exitTimer = 0f;
        private PlayerController _player;

        private float _airDuration = 0.5f;
        private GroundType _prev;


        public string AnimationName => "BigDamage";

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
            _airTimer = 0f;
            _exitTimer = 0f;
            _prev = GroundType.None;
        }

        public void Execute(PlayerController player)
        {
            var duration = player.MovementParameters.BigDamageDuration;
            var horizontalDecelerationRate = player.MovementParameters.BigDamageHorizontalDecelerationRate;

            _exitTimer += Time.deltaTime;
            if (_exitTimer >= duration)
            {
                this.TransitionToDefaultState(player);
                return;
            }

            var groundSensorResult = player.Sensor.CalculateGroundState(player);

            if (_airDuration > _airTimer || player.MovementParameters.Velocity.y >= 0f)
            {
                _airTimer += Time.deltaTime;
                InAir.Move(player, false);
                _prev = GroundType.InAir;
                return;
            }

            if (_prev != groundSensorResult.GroundType &&
                groundSensorResult.GroundType == GroundType.Ground)
            {
                player.MovementParameters.Velocity = new Vector2(player.MovementParameters.Velocity.x, 0);
            }
            switch (groundSensorResult.GroundType)
            {
                case GroundType.Ground: Grounded.Move(player, false, false); break;
                case GroundType.Abyss: Abyss.Move(player); break;
                case GroundType.SteepSlope: SteepSlope.Move(player); break;
                case GroundType.InAir: InAir.Move(player, false); break;
            }
            _prev = groundSensorResult.GroundType;
        }

        public void Exit(PlayerController player)
        {

        }
    }
}
