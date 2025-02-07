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
        private string _animationName;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController controller)
        {

        }

        public override void Execute(PlayerController player, BullvarController controller)
        {

        }

        public override void Exit(PlayerController player, BullvarController controller)
        {

        }
    }
}