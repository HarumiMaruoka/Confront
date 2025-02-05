using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public class ProjectileMotion : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _gravity = -9.8f;

        [Header("On Hit")]
        [SerializeField] private GameObject _hit;
        [SerializeField] private ParticleSystem _hitPS;

        [SerializeField] private ParticleSystem[] _detached;
        [SerializeField] private ParticleSystem _projectilePS;

        [Header("Damage")]
        [SerializeField] private float _baseDamage = 10;
        [SerializeField] private float _damageFactor = 1f;
        [Space]
        [SerializeField] private Vector2 _knockbackDirection = new Vector2(1, 1);
        [SerializeField] private float _knockbackForce;

        private float _actorAttackPower;

        private Transform _target;
        private float _flightDuration = 2.0f;  // 飛行にかける時間（秒）

        private Vector3 _lastPosition;
        private Vector2 _startPosition;
        private Vector2 _initialVelocity;
        private float _elapsedTime = 0f;

        public event Action<ProjectileMotion> OnCompleted;

        public ProjectileMotion Launch(float actorAttackPower, Vector2 startPosition, Transform target, float flightDuration)
        {
            this._actorAttackPower = actorAttackPower;
            this._elapsedTime = 0f;
            this.transform.position = startPosition;
            this._lastPosition = startPosition;
            this._startPosition = startPosition;
            this._target = target;
            this._flightDuration = flightDuration;
            Initialize();
            return this;
        }

        private void Initialize()
        {
            // ターゲットが指定されていなければエラー表示
            if (_target == null)
            {
                Debug.LogError("ターゲットが指定されていません！");
                enabled = false;
                return;
            }

            // 発射開始位置を保存
            _startPosition = transform.position;

            // 水平方向の初速度：等速運動でターゲットに到達するための値
            float vx = (_target.position.x - _startPosition.x) / _flightDuration;

            // 垂直方向の初速度は、重力加速度を考慮して計算する
            // 公式: target.y = start.y + v0y * t + (1/2)*g*t^2　より
            float vy = (_target.position.y - _startPosition.y - 0.5f * _gravity * _flightDuration * _flightDuration) / _flightDuration;

            _initialVelocity = new Vector2(vx, vy);
        }

        private void Update()
        {
            UpdatePosition();
            HandleHitDetection();
        }

        private void UpdatePosition()
        {
            // 座標を更新
            _elapsedTime += Time.deltaTime;
            // 位置の更新：初期位置 + 初速度 * 時間 + 1/2 * 加速度 * 時間^2
            transform.position = _startPosition + _initialVelocity * _elapsedTime + 0.5f * new Vector2(0, _gravity) * _elapsedTime * _elapsedTime;
        }

        private void HandleHitDetection()
        {
            var layerMask = LayerUtility.PlayerLayerMask | LayerUtility.GroundLayerMask;

            Physics.SphereCast(_lastPosition, _radius, transform.position - _lastPosition, out var hit, Vector3.Distance(_lastPosition, transform.position), layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    var damageValue = _baseDamage + _actorAttackPower * _damageFactor;
                    var sign = Mathf.Sign(transform.position.x - _lastPosition.x);
                    var damageVector = HitBoxBase.CalcDamageVector(_knockbackDirection, _knockbackForce, sign);
                    damageable.TakeDamage(damageValue, damageVector);
                }
                HandleCollision(hit.point);
            }

            _lastPosition = transform.position;
        }

        private void HandleCollision(Vector3 hitPosition)
        {
            transform.position = hitPosition;

            _projectilePS.Stop();
            _projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // ヒットエフェクトの再生
            _hit.transform.position = hitPosition;
            _hitPS.transform.position = hitPosition;
            _hitPS.transform.parent = null;
            _hitPS.Play();

            //var hitPS = Instantiate(_hitPS, hitPosition, Quaternion.identity);
            //hitPS.Play();

            // パーティクルの停止
            foreach (var detachedPrefab in _detached) detachedPrefab.Stop();

            this.enabled = false;
            OnCompleted?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            if (!enabled) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_lastPosition, _radius);
            Gizmos.DrawWireSphere(transform.position, _radius);

            Gizmos.DrawLine(_lastPosition, transform.position);
        }
    }
}