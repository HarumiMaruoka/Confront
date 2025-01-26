using Confront.AttackUtility;
using Confront.Enemy;
using Confront.GameUI;
using System;
using UnityEngine;

namespace Confront.Boss
{
    public class BossBase : EnemyBase
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

        protected virtual void OnDie()
        {

        }

        private bool _isInvincible = false;

        private float? _damage = null;
        private Vector2 _damagePosition;

        protected virtual void Update()
        {
            if (_damage.HasValue)
            {
                Debug.Log($"ボスに{_damage.Value}のダメージを与えた");
                Health -= _damage.Value;

                DamageDisplaySystem.Instance.ShowDamage((int)_damage.Value, _damagePosition);

                if (Health <= 0)
                {
                    OnDie();
                    _isInvincible = true;
                }

                _damage = null;
            }
        }

        public void TakeDamage(float damage, Vector2 damagePosition, Vector2 damageVector)
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

        protected override string CreateSaveData()
        {
            Debug.LogWarning("未実装");
            return "";
        }

        protected override void Load(string saveData)
        {
            Debug.LogWarning("未実装");
        }
    }
}