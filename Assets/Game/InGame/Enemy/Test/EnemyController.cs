using Confront.GameUI;
using Confront.AttackUtility;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public float health = 100f;

        public void TakeDamage(float damage)
        {
            health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, transform.position);
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}