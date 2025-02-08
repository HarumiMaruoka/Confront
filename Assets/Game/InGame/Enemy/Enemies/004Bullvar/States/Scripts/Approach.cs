using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // プレイヤーに近づく。
    // プレイヤーに近づいたらAttack,Blockに遷移する。
    // ダメージを食らったら確率でDamageに遷移する。
    // 視界からプレイヤーが消えたらIdleかWanderに遷移する。
    [CreateAssetMenu(fileName = "ApproachState", menuName = "ConfrontSO/Enemy/Bullvar/States/ApproachState")]
    public class Approach : BullvarState
    {
        [Header("Animation")]
        [SerializeField]
        private string _animationName = "Run";

        [Header("Movement")]
        public float Acceleration = 12f;
        public float MaxSpeed = 10f;

        [Header("Transition")]
        public float CombatDistance = 6f;
        [Range(0f, 1f)]
        public float AttackTransitionProbability = 0.5f;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {

        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            if (!bullvar.Eye.IsVisiblePlayer(bullvar.transform, player))
            {
                bullvar.ChangeState<Wander>();
                return;
            }

            var sqrDistance = (player.transform.position - bullvar.transform.position).sqrMagnitude;
            if (sqrDistance < CombatDistance * CombatDistance)
            {
                var random = UnityEngine.Random.value;
                if (random < AttackTransitionProbability)
                {
                    bullvar.ChangeState<Attack>();
                }
                else
                {
                    bullvar.ChangeState<Block>();
                }
                return;
            }

            var dir = (player.transform.position - bullvar.transform.position).x >= 0f ? 1f : -1f;
            var xSpeed = Mathf.MoveTowards(bullvar.Velocity.x, MaxSpeed * dir, Time.deltaTime * Acceleration);
            var ySpeed = bullvar.Velocity.y;
            bullvar.Velocity = new Vector3(xSpeed, ySpeed, 0);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}