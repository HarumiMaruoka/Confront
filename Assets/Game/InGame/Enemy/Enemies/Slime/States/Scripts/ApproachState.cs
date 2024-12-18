using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    // プレイヤーやターゲットを感知すると、ゆっくりとその方向に向かって移動する行動です。攻撃の準備段階にあたります。
    [CreateAssetMenu(fileName = "ApproachState", menuName = "Enemy/Slimey/States/ApproachState")]
    public class ApproachState : SlimeyState
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