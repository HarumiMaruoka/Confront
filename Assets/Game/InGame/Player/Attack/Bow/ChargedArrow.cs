using Confront.AttackUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class ChargedArrow : ChargedProjectile
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        [Header("Spawn Offset")]
        [SerializeField]
        private Vector2 _spawnOffset = new Vector2(0f, 1f);
        [Header("Direction")]
        [SerializeField]
        private Vector2 _direction = new Vector2(1f, 0f);

        [Header("Speed")]
        [SerializeField]
        private float _minSpeed = 10;
        [SerializeField]
        private float _maxSpeed = 20;

        [Header("Life Time")]
        [SerializeField]
        private float _minLifeTime = 0.5f;
        [SerializeField]
        private float _maxLifeTime = 0.5f;

        [Header("Damage")]
        [SerializeField]
        private float _baseDamage = 10;
        [SerializeField]
        private float _minFactor = 0.5f;
        [SerializeField]
        private float _maxFactor = 1.5f;

        private PlayerController _player;
        private float _factor;
        private HashSet<int> _alreadyHits = new HashSet<int>();

        private static LayerMask CollisionDestructionLayer;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            CollisionDestructionLayer = LayerMask.GetMask("Ground");
        }

        public override void Initialize(PlayerController player, float chargeAmount)
        {
            transform.position += new Vector3(_spawnOffset.x, _spawnOffset.y);

            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            _direction = _direction.normalized;
            var velocity = new Vector3(_direction.x * sign, _direction.y) * Mathf.Lerp(_minSpeed, _maxSpeed, chargeAmount);

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            _player = player;
            _factor = Mathf.Lerp(_minFactor, _maxFactor, chargeAmount);
            _rigidbody.velocity = velocity;

            var lifeTime = Mathf.Lerp(_minLifeTime, _maxLifeTime, chargeAmount);
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            // プレイヤーとヒットしないようにレイヤーコリジョンマトリックスを設定すること。
            // 壁にあたったら消滅する。
            if (CollisionDestructionLayer == (CollisionDestructionLayer | (1 << other.gameObject.layer)))
            {
                Destroy(gameObject);
            }

            // すでにヒットしたオブジェクトは無視する。
            if (!_alreadyHits.Add(other.gameObject.GetInstanceID())) return;

            // ダメージを与える。
            if (other.TryGetComponent(out IDamageable damageable))
            {
                var damage = _baseDamage + _player.CharacterStats.AttackPower * _factor;
                var damageVector = transform.right * _rigidbody.velocity.magnitude;
                damageable.TakeDamage(damage, damageVector);
            }
        }
    }
}