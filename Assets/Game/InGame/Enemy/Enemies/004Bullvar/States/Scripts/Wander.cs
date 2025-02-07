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