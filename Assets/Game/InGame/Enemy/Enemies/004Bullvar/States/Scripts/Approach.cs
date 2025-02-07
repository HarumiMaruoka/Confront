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