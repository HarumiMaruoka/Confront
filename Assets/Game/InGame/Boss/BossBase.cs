using Confront.AttackUtility;
using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public class BossBase : EnemyBase, IDamageable
    {
        public float Health;
        public BossPart[] DestructibleParts;

        protected override void Awake()
        {
            base.Awake();
            foreach (var part in DestructibleParts)
            {
                part.Initialize(this);
                if (part is DestructiblePart destructiblePart)
                {
                    destructiblePart.OnDestructed += OnPartDestructed;
                }
            }
        }

        private void OnPartDestructed(DestructiblePart part)
        {
            Debug.Log($"部位破壊された。{part.name}");
        }

        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            Health -= attackPower;

            if (Health <= 0)
            {
                Debug.Log("ボス撃破");
            }
        }
    }
}