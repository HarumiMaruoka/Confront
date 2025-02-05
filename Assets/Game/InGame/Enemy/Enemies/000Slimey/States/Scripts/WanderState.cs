using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // スライムが特に目的もなく、ゆっくりと周囲を動き回る行動。探索や巡回のように見えることがあります。
    [CreateAssetMenu(fileName = "WanderState", menuName = "ConfrontSO/Enemy/Slimey/States/WanderState")]
    public class WanderState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "RunFWD";

        [Header("移動系")]
        public float MoveSpeed = 2f;
        public float TargetSpeed = 5f;
        public float SpeedLimit = 5f;
        public float NoiseScale = 1f;

        private float _timeOffsetX;
        private float _timeOffsetY;

        [Header("時間")]
        public float MinDuration = 1f;
        public float MaxDuration = 3f;

        private float _timer;
        private float _duration;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timeOffsetX = UnityEngine.Random.Range(-100f, 100f);
            _timeOffsetY = UnityEngine.Random.Range(-100f, 100f);
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0f;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            if (slimey.Eye.IsVisiblePlayer(slimey.transform, player))
            {
                slimey.ChangeState<ApproachState>();
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                slimey.ChangeState<IdleState>();
                return;
            }

            float x = Mathf.PerlinNoise(Time.time * NoiseScale + _timeOffsetX, Time.time * NoiseScale + _timeOffsetY);
            float xSpeed = Mathf.Lerp(-TargetSpeed, TargetSpeed, x);

            xSpeed = Mathf.Clamp(xSpeed, -SpeedLimit, SpeedLimit);

            var ySpeed = slimey.Rigidbody.velocity.y;
            slimey.Rigidbody.velocity = new Vector3(xSpeed, ySpeed, 0f);
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}