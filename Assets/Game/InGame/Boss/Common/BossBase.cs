﻿using Confront.AttackUtility;
using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public class BossBase : EnemyBase, IDamageable
    {
        public float Health;
        public BossPart[] BossParts;

        protected override void Awake()
        {
            base.Awake();
            foreach (var part in BossParts)
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

        private bool _isInvincible = false;

        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            if (_isInvincible) return;
            Health -= attackPower;

            if (Health <= 0)
            {
                Debug.Log("ボス撃破");
                _isInvincible = true;
            }
        }
    }
}