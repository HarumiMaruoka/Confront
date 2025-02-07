using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    // プレイヤーやターゲットを感知すると、その方向に向かって移動する行動です。攻撃の準備段階にあたります。
    [CreateAssetMenu(fileName = "ApproachState", menuName = "ConfrontSO/Enemy/Slimey/States/ApproachState")]
    public class ApproachState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "RunFWD";

        public float AttackRange = 1.0f;
        public float DisengageDistance = 5.0f;
        public float ApproachAcceleration = 0.1f;
        public float ApproachMaxSpeed = 1.0f;

        [Header("ブロック状態に遷移する確率：ブロックステートが設定されている場合のみ有効。")]
        [Range(0, 1)]
        public float TransitionProbabilityToBlockState = 0.1f;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {

        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            // プレイヤーが見えなくなったら待機状態に戻る。
            if (!slimey.Eye.IsVisiblePlayer(slimey.transform, player))
            {
                slimey.ChangeState<IdleState>();
                return;
            }

            var sqrDistance = (player.transform.position - slimey.transform.position).sqrMagnitude;
            if (sqrDistance < AttackRange * AttackRange)
            {
                var random = UnityEngine.Random.value;
                if (slimey.HasBlockState && random < TransitionProbabilityToBlockState)
                {
                    slimey.ChangeState<BlockState>();
                    return;
                }

                slimey.ChangeState<AttackState>();
            }
            else if (sqrDistance > DisengageDistance * DisengageDistance)
            {
                slimey.ChangeState<IdleState>();
            }
            else
            {
                var dir = (player.transform.position - slimey.transform.position).x >= 0f ? 1f : -1f;
                var xSpeed = Mathf.MoveTowards(slimey.Rigidbody.velocity.x, ApproachMaxSpeed * dir, Time.deltaTime * ApproachAcceleration);
                var ySpeed = slimey.Rigidbody.velocity.y;

                slimey.Rigidbody.velocity = new Vector3(xSpeed, ySpeed, 0);
            }
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}