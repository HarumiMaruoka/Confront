using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 防御する。
    // 一定時間経過したらIdle,Wanderに遷移する。
    [CreateAssetMenu(fileName = "BlockState", menuName = "ConfrontSO/Enemy/Bullvar/States/BlockState")]
    public class BlockState : BullvarState
    {
        [Header("Animation")]
        [SerializeField]
        private string _animationName = "Block";

        [Header("Movement")]
        public float Deceleration = 8f;

        [Header("Duration")]
        public float MinDuration = 1f;
        public float MaxDuration = 3f;

        [Header("Defense")]
        public float DefenseBoost = 1f;

        public float IdleTransitionProbability = 0.5f;

        private float _timer;
        private float _duration;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _timer = 0;
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            bullvar.Stats.Defense += DefenseBoost;
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                var random = UnityEngine.Random.value;
                if (random < IdleTransitionProbability)
                {
                    bullvar.ChangeState<IdleState>();
                }
                else
                {
                    bullvar.ChangeState<WanderState>();
                }
            }

            bullvar.Velocity.x = Mathf.Lerp(bullvar.Velocity.x, 0, Time.deltaTime * Deceleration);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {
            bullvar.Stats.Defense -= DefenseBoost;
        }
    }
}