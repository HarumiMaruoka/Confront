using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 一定時間経過したらApproach,Blockに遷移する。
    [CreateAssetMenu(fileName = "DamageState", menuName = "ConfrontSO/Enemy/Bullvar/States/DamageState")]
    public class DamageState : BullvarState
    {
        [SerializeField]
        private string _animationName;

        public float Deceleration = 8f;
        public float Duration = 1f;
        public float ApproachTransitionProbability = 0.5f;

        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _timer = 0;
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                var random = UnityEngine.Random.value;
                if (random < ApproachTransitionProbability)
                {
                    bullvar.ChangeState<ApproachState>();
                }
                else
                {
                    bullvar.ChangeState<BlockState>();
                }
            }

            bullvar.Velocity = Vector2.Lerp(bullvar.Velocity, Vector2.zero, Deceleration * Time.deltaTime);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}