using Confront.AttackUtility;
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
        [SerializeField]
        private Vector2 _damageDirection;
        [SerializeField]
        private float _damageForce;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                var damageVector = HitBoxBase.CalcDamageVector(_damageDirection, _damageForce, 1);
                player.TakeDamage(_damage, damageVector);
            }
            Destroy(gameObject);
        }
    }
}