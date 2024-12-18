using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // 	スライムがその場で動かず、待機している状態。特に敵やプレイヤーが近づいていない時に見せる行動です。
    [CreateAssetMenu(fileName = "IdleState", menuName = "Enemy/Slimey/States/IdleState")]
    public class IdleState : SlimeyState
    {
        public float MinDuration = 1f;
        public float MaxDuration = 2f;

        private float _duration;
        private float _timer;

        public float Deceleration = 10f;

        public override string AnimationName => "IdleNormal";

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                slimey.ChangeState<WanderState>();
            }

            var xSpeed = Mathf.MoveTowards(slimey.Rigidbody.velocity.x, 0, Deceleration * Time.deltaTime);
            slimey.Rigidbody.velocity = new Vector2(xSpeed, slimey.Rigidbody.velocity.y);
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}