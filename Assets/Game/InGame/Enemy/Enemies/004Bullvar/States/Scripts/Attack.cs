using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 攻撃する。
    // ダメージを食らったら確率でDamageに遷移する。
    // アニメーションが終了したらBlockかApproachに遷移する。
    [CreateAssetMenu(fileName = "AttackState", menuName = "ConfrontSO/Enemy/Bullvar/States/AttackState")]
    public class Attack : BullvarState
    {
        [SerializeField]
        private string _animationName = "Attack_01";

        public float Deceleration = 8f;

        public float blockTransitionProbability = 0.5f;
        public float Duration = 3f;

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
                if (random < blockTransitionProbability)
                {
                    bullvar.ChangeState<Block>();
                }
                else
                {
                    bullvar.ChangeState<Approach>();
                }
            }

            bullvar.Velocity.x = Mathf.Lerp(bullvar.Velocity.x, 0, Time.deltaTime * Deceleration);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}