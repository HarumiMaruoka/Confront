using Confront.Player;
using System;
using UnityEngine;

namespace Confront.StageGimmick.Test
{
    public class HealObject : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                var maxHealth = player.HealthManager.MaxHealth;
                player.HealthManager.Heal(maxHealth);
            }
        }
    }
}