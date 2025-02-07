using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 一定時間経過したらApproach,Damageに遷移する。
    [CreateAssetMenu(fileName = "DamageState", menuName = "ConfrontSO/Enemy/Bullvar/States/DamageState")]
    public class Damage : BullvarState
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