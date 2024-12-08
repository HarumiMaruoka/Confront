using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Item
{
    public class HealPotion : ItemEffect
    {
        public override void ApplyEffect(PlayerController player)
        {
            player.HealthManager.Heal(10);
        }
    }
}