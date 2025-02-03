using Confront.AttackUtility;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy.Muscomorph
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 0.5f;
        [SerializeField]
        private float _minDuration = 2f;
        [SerializeField]
        private float _maxDuration = 5f;

        [Header("Damage")]
        [SerializeField]
        private float _baseDamage = 10;
        [SerializeField]
        private float _damageFactor = 1f;

        [SerializeField]
        private Vector2 _knockbackDirection = new Vector2(1, 1);
        [SerializeField]
        private float _knockbackForce;

        private ProjectileMotion _projectileMotion;

        public event Action<Bullet> OnCompleted;
        public float _actorAttackPower;

        public void Initialize(Vector3 position, float actorAttackPower)
        {
            gameObject.SetActive(true);

            if (_projectileMotion == null)
            {
                _projectileMotion = this.gameObject.AddComponent<ProjectileMotion>();
                _projectileMotion.OnComplete += OnComplete;
            }

            var duration = UnityEngine.Random.Range(_minDuration, _maxDuration);
            _projectileMotion.Launch(position, Player.PlayerController.Instance.transform, duration);
            _lastPos = transform.position;

            _actorAttackPower = actorAttackPower;
        }

        private Vector3 _lastPos;

        private void Update()
        {
            var playerLayer = LayerUtility.PlayerLayerMask;
            var groundLayer = LayerUtility.GroundLayerMask;

            var hit = Physics2D.CircleCast(_lastPos, _radius, transform.position - _lastPos, Vector2.Distance(_lastPos, transform.position), playerLayer | groundLayer);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    var damageValue = _baseDamage + _actorAttackPower * _damageFactor;
                    var sign = Mathf.Sign(transform.position.x - _lastPos.x);
                    var damageVector = HitBoxBase.CalcDamageVector(_knockbackDirection, _knockbackForce, sign);
                    damageable.TakeDamage(damageValue, damageVector);
                }

                gameObject.SetActive(false);
                OnCompleted?.Invoke(this);
            }

            _lastPos = transform.position;
        }

        private void OnComplete(ProjectileMotion motion)
        {
            gameObject.SetActive(false);
            OnCompleted?.Invoke(this);
        }
    }
}