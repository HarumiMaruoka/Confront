using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 防御する。
    // 一定時間経過したらIdleに遷移する。
    [CreateAssetMenu(fileName = "BlockState", menuName = "ConfrontSO/Enemy/Bullvar/States/BlockState")]
    public class Block : BullvarState
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