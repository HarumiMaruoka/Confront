using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // ターゲットに向かって勢いよく突進し、ぶつかることでダメージを与える攻撃行動です。スライムの攻撃の中で最も特徴的な行動です。
    [CreateAssetMenu(fileName = "AttackState", menuName = "ConfrontSO/Enemy/Slimey/States/AttackState")]
    public class AttackState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "Attack02";

        public float Cooldown = 1.0f;
        public float AttackRange = 1.0f;

        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timer = 0;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            if (!slimey.Eye.IsVisiblePlayer(slimey.transform, player))
            {
                slimey.ChangeState<IdleState>();
            }

            var sqrDistance = (player.transform.position - slimey.transform.position).sqrMagnitude;
            if (sqrDistance > AttackRange * AttackRange)
            {
                slimey.ChangeState<ApproachState>();
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                // 体当たりする。
                slimey.Animator.CrossFade(AnimationName, 0.1f);
                _timer = Cooldown;
            }
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}