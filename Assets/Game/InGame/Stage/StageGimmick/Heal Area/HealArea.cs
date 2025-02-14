using Confront.Audio;
using Confront.Enemy;
using Confront.Player;
using Confront.SaveSystem;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.StageGimmick.Test
{
    public class HealArea : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 1.0f;
        [SerializeField]
        private AudioClip _healSound;

        private bool ContainsPlayer => Physics.CheckSphere(transform.position, _radius, LayerUtility.PlayerLayerMask);
        private bool _isOn = false; // 何度も回復しないようにするためのフラグ

        private void Update()
        {
            if (ContainsPlayer)
            {
                if (_isOn) return;
                var maxHealth = PlayerController.Instance.HealthManager.MaxHealth;
                PlayerController.Instance.HealthManager.Heal(maxHealth);
                if (_healSound) AudioManager.PlaySE(_healSound);
                _isOn = true;

                SaveDataController.Loaded?.EnemyData?.Clear(); // 敵のデータを削除
                EnemyManager.OnEnemiesReset?.Invoke();
                return;
            }
            _isOn = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = ContainsPlayer ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}