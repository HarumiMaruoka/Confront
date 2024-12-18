using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // スライムが特に目的もなく、ゆっくりと周囲を動き回る行動。探索や巡回のように見えることがあります。
    [CreateAssetMenu(fileName = "WanderState", menuName = "Enemy/Slimey/States/WanderState")]
    public class WanderState : SlimeyState
    {
        [Header("移動系")]
        public float MoveSpeed = 2f;
        public float RangeMin;
        public float RangeMax;
        public float NoiseScale = 1f;

        private float _timeOffsetX;

        [Header("時間")]
        public float MinDuration = 1f;
        public float MaxDuration = 3f;

        private float _timer;
        private float _duration;

        public override string AnimationName => "RunFWD";

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timeOffsetX = UnityEngine.Random.Range(0f, 100f);
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0f;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                slimey.ChangeState<IdleState>();
                return;
            }

            float x = Mathf.PerlinNoise(Time.time * NoiseScale + _timeOffsetX, 0f);
            float xSpeed = Mathf.Lerp(RangeMin, RangeMax, x);

            var ySpeed = slimey.Rigidbody.velocity.y;
            slimey.Rigidbody.velocity = new Vector3(xSpeed, ySpeed, 0f);
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}