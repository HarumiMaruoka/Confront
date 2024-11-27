using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "ChargedAttackState", menuName = "Confront/Player/Combo/ChargedAttackState")]
    public class ChargedAttackState : AttackBehaviour
    {
        public override string AnimationName => "ChargedAttack";

        public override void Enter(PlayerController player)
        {

        }

        public override void Execute(PlayerController player)
        {

        }

        public override void Exit(PlayerController player)
        {

        }
    }
}