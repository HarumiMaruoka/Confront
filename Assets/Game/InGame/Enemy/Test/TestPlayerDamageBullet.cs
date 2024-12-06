using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Test
{
    // テスト用：プレイヤーにダメージを与える弾
    [RequireComponent(typeof(Rigidbody))]
    public class TestPlayerDamageBullet : MonoBehaviour
    {
        [SerializeField]
        private float _damage = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                player.HealthManager.Damage(_damage);
            }
            Destroy(gameObject);
        }
    }
}