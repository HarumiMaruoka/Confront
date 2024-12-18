using System;
using UnityEngine;

namespace Confront.Player
{
    // 大きくノックバックするダメージ状態
    public class BigDamage : IState
    {
        public string AnimationName => "BigDamage";

        public Vector2 DamageDirection;

        public void Enter(PlayerController player)
        {

        }

        public void Execute(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {

        }
    }
}
