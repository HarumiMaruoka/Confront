using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // ターゲットに向かって勢いよく突進し、ぶつかることでダメージを与える攻撃行動です。スライムの攻撃の中で最も特徴的な行動です。
    [CreateAssetMenu(fileName = "AttackState", menuName = "Enemy/Slimey/States/AttackState")]
    public class AttackState : SlimeyState
    {
        public override string AnimationName => string.Empty;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {

        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {

        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}