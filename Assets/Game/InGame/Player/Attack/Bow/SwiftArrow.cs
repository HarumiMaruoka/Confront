using Confront.AttackUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class SwiftArrow : Projectile
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
        private float _speed = 10;

        [Header("Life Time")]
        [SerializeField]
        private float _lifeTime = 0.5f;

        [Header("Damage")]
        [SerializeField]
        private float _baseDamage = 10;
        [SerializeField]
        private float _damageFactor = 1f;

        private PlayerController _player;
        private HashSet<int> _alreadyHits = new HashSet<int>();

        private static LayerMask CollisionDestructionLayer;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            CollisionDestructionLayer = LayerMask.GetMask("Ground");
        }

        public override void Initialize(PlayerController player)
        {
            transform.position += new Vector3(_spawnOffset.x, _spawnOffset.y);

            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            _direction = _direction.normalized;
            var velocity = new Vector3(_direction.x * sign, _direction.y) * _speed;

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            _player = player;
            _rigidbody.velocity = velocity;

            Destroy(gameObject, _lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            // プレイヤーの弾であれば プレイヤーとヒットしないようにレイヤーコリジョンマトリックスを設定すること。
            // 敵の弾であれば 敵とヒットしないようにレイヤーコリジョンマトリックスを設定すること。
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
                var damage = _baseDamage + _player.CharacterStats.AttackPower * _damageFactor;
                var damageVector = transform.right * _rigidbody.velocity.magnitude;
                damageable.TakeDamage(damage, damageVector, other.bounds.center);
            }
        }
    }
}