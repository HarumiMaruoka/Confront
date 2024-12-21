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
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private Vector2 _damageDirection;
        [SerializeField]
        private float _damageForce;

        public event Action<TestPlayerDamageBullet> OnHit;

        private void OnEnable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                var damageVector = HitBoxBase.CalcDamageVector(_damageDirection, _damageForce, 1);
                player.TakeDamage(_damage, damageVector);
            }
            OnHit?.Invoke(this);
        }
    }
}