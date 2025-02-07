using Confront.AttackUtility;
using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy.FrostLilith
{
    public class IceAttack : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField]
        private float _damageValue = 10f;
        [Space]
        [SerializeField]
        private Vector2 _knockbackDirection = new Vector2(1, 1);
        [SerializeField]
        private float _knockbackForce = 10f;
        [Header("Time")]
        [SerializeField]
        private float _activationTime = 0.5f;
        [SerializeField]
        private float _deactivationTime = 0.5f;
        [Space]
        [SerializeField]
        private float _lifeTime = 3f;
        [Header("HitBox")]
        [SerializeField]
        private Vector3 _hitBoxHalfSize = new Vector3(1, 1, 1);
        [SerializeField]
        private Vector3 _hitBoxOffset = new Vector3(0, 0, 0);

        public float AttackDirection;
        public float ActorAttackPower;

        private float _elapsedTime = 0f;
        private bool _isHit = false;

        private bool IsActive => _activationTime <= _elapsedTime && _elapsedTime <= _deactivationTime;
        private LayerMask PlayerLayer => LayerUtility.PlayerLayerMask;
        private bool ContainsPlayer => Physics.CheckBox(transform.position + _hitBoxOffset, _hitBoxHalfSize, Quaternion.identity, PlayerLayer);

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (IsActive) PerformAttack();
        }

        private void PerformAttack()
        {
            if (_isHit) return;
            if (ContainsPlayer)
            {
                _isHit = true;
                var player = PlayerController.Instance;
                var damageVector = HitBoxBase.CalcDamageVector(_knockbackDirection, _knockbackForce, AttackDirection);
                player.TakeDamage(_damageValue, damageVector);
            }
        }

        private void OnDrawGizmos()
        {
            if (ContainsPlayer) Gizmos.color = Color.red;
            else if (IsActive) Gizmos.color = Color.blue;
            else Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + _hitBoxOffset, _hitBoxHalfSize * 2f);
        }
    }
}