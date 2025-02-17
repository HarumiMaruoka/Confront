using Confront.AttackUtility;
using Confront.Enemy;
using Confront.GameUI;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public abstract class BossBase : EnemyBase
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

        protected virtual void OnPartDestructed(DestructiblePart destructedPart)
        {
            Debug.Log($"部位破壊された。{destructedPart.name}");
        }

        protected abstract void OnDie();

        private bool _isInvincible = false;

        private float? _damage = null;
        private Vector2 _damagePosition;

        protected virtual void Update()
        {
            if (_damage.HasValue)
            {
                if (Health <= 0)
                {
                    OnDie();
                    _isInvincible = true;
                }

                Health -= _damage.Value;
                DamageDisplaySystem.Instance.ShowDamage((int)_damage.Value, _damagePosition);
                _damage = null;
            }
        }

        public void TakeDamage(float damage, Vector2 damageVector, Vector2 damagePosition)
        {
            if (_isInvincible) return;
            if (_damage.HasValue)
            {
                if (_damage.Value < damage)
                {
                    _damage = damage;
                    _damagePosition = damagePosition;
                }
            }
            else
            {
                _damage = damage;
                _damagePosition = damagePosition;
            }
        }
    }
}