using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ConfrontSO/Enemy/Stats")]
    public class EnemyStats : ScriptableObject
    {
        public float MaxHealth = 100f;
        public float AttackPower = 10f;
        public float Defense = 0f;

        private float _health;
        public event Action<float> OnLifeChanged;
        public float Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0f, MaxHealth);
                OnLifeChanged?.Invoke(_health);
            }
        }
    }
}