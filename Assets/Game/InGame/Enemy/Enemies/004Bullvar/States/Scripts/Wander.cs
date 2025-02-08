using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // ランダムな方向に移動する。
    // 一定時間経過後、Idleに遷移する。
    // 視界にプレイヤーが入ったらApproachに遷移する。
    // ダメージを食らったら確率でDamageに遷移する。
    [CreateAssetMenu(fileName = "WanderState", menuName = "ConfrontSO/Enemy/Bullvar/States/WanderState")]
    public class Wander : BullvarState
    {
        [Header("Animation")]
        [SerializeField]
        private string _animationName = "Walk_F";

        [Header("Duration")]
        public float MinDuration = 1f;
        public float MaxDuration = 3f;

        [Header("Movement")]
        public float MoveSpeedRange = 12f;
        public float SpeedLimit = 6f;
        public float NoiseScale = 0.1f;

        private float _timeOffsetX;
        private float _timeOffsetY;

        private float _timer;
        private float _duration;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _timeOffsetX = UnityEngine.Random.Range(-100f, 100f);
            _timeOffsetY = UnityEngine.Random.Range(-100f, 100f);
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0f;
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            if (bullvar.Eye.IsVisiblePlayer(bullvar.transform, player))
            {
                bullvar.ChangeState<Approach>();
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                bullvar.ChangeState<Idle>();
                return;
            }

            float x = Mathf.PerlinNoise(Time.time * NoiseScale + _timeOffsetX, Time.time * NoiseScale + _timeOffsetY);
            float xSpeed = Mathf.Lerp(-MoveSpeedRange, MoveSpeedRange * 1.10f, x); // 少し左側に寄りがちなので、右に少しバイアスをかける
            xSpeed = Mathf.Abs(xSpeed) > 0.08f ? xSpeed : 0f;
            xSpeed = Mathf.Clamp(xSpeed, -SpeedLimit, SpeedLimit);

            var ySpeed = bullvar.Velocity.y;
            bullvar.Velocity = new Vector2(xSpeed, ySpeed);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}