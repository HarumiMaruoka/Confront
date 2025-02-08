using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 減速、停止させる。
    // 一定時間経過後、Wanderに遷移する。
    // 視界にプレイヤーが入ったらApproachに遷移する。
    [CreateAssetMenu(fileName = "IdleState", menuName = "ConfrontSO/Enemy/Bullvar/States/IdleState")]
    public class Idle : BullvarState
    {
        [Header("Animation")]
        [SerializeField]
        private string _animationName = "Idle";

        [Header("Duration")]
        public float MinDuration = 1f;
        public float MaxDuration = 3f;

        [Header("Deceleration")]
        public float Deceleration = 8f;

        public override string AnimationName => _animationName;

        private float _duration;
        private float _timer;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0;
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            _timer += Time.deltaTime;

            if (bullvar.Eye.IsVisiblePlayer(bullvar.transform, player))
            {
                bullvar.ChangeState<Approach>();
                return;
            }

            bullvar.Velocity.x = Mathf.Lerp(bullvar.Velocity.x, 0, Time.deltaTime * Deceleration);

            if (_timer >= _duration)
            {
                bullvar.ChangeState<Wander>();
            }
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}