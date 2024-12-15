using Confront.AttackUtility;
using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public class Bow : ChargedProjectile
    {
        [SerializeField]
        private float _lifeTime = 1f;
        [SerializeField]
        private float _radius = 0.5f;

        [SerializeField]
        private float _minSpeed = 5f;
        [SerializeField]
        private float _maxSpeed = 30f;

        [SerializeField]
        private float _baseDamage = 10f;
        [SerializeField]
        private float _chargeDamageMinMultiplier = 1f;
        [SerializeField]
        private float _chargeDamageMaxMultiplier = 5f;

        private PlayerController _player;
        private float _chargeDamageMultiplier;
        private float _speed;
        private LayerMask _layerMask;
        private Vector3 _previousPosition;

        public override void Initialize(PlayerController player, float chargeAmount)
        {
            _player = player;
            _chargeDamageMultiplier = Mathf.Lerp(_chargeDamageMinMultiplier, _chargeDamageMaxMultiplier, chargeAmount);
            _layerMask = LayerMask.GetMask("Enemy");
            _speed = Mathf.Lerp(_minSpeed, _maxSpeed, chargeAmount);
            _previousPosition = transform.position;
            transform.rotation = player.transform.rotation;
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;

            var hits = Physics.SphereCastAll(_previousPosition, _radius, transform.position - _previousPosition, Vector3.Distance(transform.position, _previousPosition), _layerMask, QueryTriggerInteraction.Ignore);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    var damage = _player.CharacterStats.AttackPower + _baseDamage * _chargeDamageMultiplier;
                    damageable.TakeDamage(damage);
                }
            }

            _previousPosition = transform.position;
        }
    }
}