using System;
using UnityEngine;

namespace Confront.Player
{
    // 少しノックバックするダメージ状態
    public class SmallDamage : IState
    {
        private float _timer = 0f;
        private PlayerController _player;

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
                _player.DirectionController.IsEnable = false;
                _player.MovementParameters.Velocity = value;
            }
        }

        public void Enter(PlayerController player)
        {
            _player = player;
            _timer = 0f;
        }

        public void Execute(PlayerController player)
        {
            var duration = player.MovementParameters.SmallDamageDuration;
            var horizontalDecelerationRate = player.MovementParameters.SmallDamageHorizontalDecelerationRate;

            _timer += Time.deltaTime;
            if (_timer >= duration)
            {
                this.TransitionToDefaultState(player);
            }

            var velocity = player.MovementParameters.Velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, Time.deltaTime * horizontalDecelerationRate);
            velocity.y -= player.MovementParameters.Gravity * Time.deltaTime;
            player.MovementParameters.Velocity = velocity;
        }

        public void Exit(PlayerController player)
        {
            player.DirectionController.IsEnable = true;
        }
    }
}