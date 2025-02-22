using Confront.VC;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Player
{
    // プレイヤーの健康管理を行うクラス
    public class HealthManager
    {
        public HealthManager(float initialHealth, float initialMaxHealth)
        {
            _currentHealth = initialHealth;
            _maxHealth = initialMaxHealth;
        }

        [SerializeField]
        private float _currentHealth;
        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

                OnHealthChanged?.Invoke(_currentHealth);
                if (_currentHealth <= 0) OnDeath?.Invoke();
            }
        }

        [SerializeField]
        private float _maxHealth;
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                OnMaxHealthChanged?.Invoke(_maxHealth, _currentHealth);
            }
        }

        public bool IsDead => _currentHealth <= 0;

        /// <summary> プレイヤーの体力が変更されたときに発生するイベント。 </summary>
        public event Action<float> OnHealthChanged;
        /// <summary> プレイヤーの最大体力が変更されたときに発生するイベント。第一引数に最大体力、第二引数に現在の体力が渡される。 </summary>
        public event Action<float, float> OnMaxHealthChanged;
        /// <summary> プレイヤーが死亡したときに発生するイベント。 </summary>
        public event Action OnDeath;

        public void Damage(float damage)
        {
            CurrentHealth -= damage;
            VCCommand.SendToyBoxEvent();
        }

        public void Heal(float heal)
        {
            CurrentHealth += heal;
            VCCommand.SendVCMainEvent();
        }
    }

    // プレイヤーに付与される状態異常を表すクラス
    public class StatusEffect
    {
        public StatusEffect(float damagePerSecond, float duration)
        {
            DamagePerSecond = damagePerSecond;
            Duration = duration;
        }

        public float DamagePerSecond;
        public float Duration;

        public async UniTask Run(HealthManager target, Action OnCompleted)
        {
            for (float t = 0f; t < Duration; t += Time.deltaTime)
            {
                if (target.IsDead) break;
#if UNITY_EDITOR
                // エディターが停止した場合は処理を中断する。
                if (!UnityEditor.EditorApplication.isPlaying) return;
#endif

                target.CurrentHealth -= DamagePerSecond * Time.deltaTime;
                await UniTask.Yield();
            }
            OnCompleted?.Invoke();
        }
    }
}