using Confront.AttackUtility;
using Confront.Enemy;
using Confront.GameUI;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public class BossPart : MonoBehaviour, IDamageable
    {
        public float Defense;

        public BossBase Boss { get; private set; }

        public GameObject Owner => Boss.gameObject;

        public void Initialize(BossBase bossBase)
        {
            this.Boss = bossBase;
        }

        public virtual void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point)
        {
            var damage = EnemyBase.DefaultCalculateDamage(attackPower, Defense);
            Boss.TakeDamage(damage, damageVector, point);
        }
    }
}