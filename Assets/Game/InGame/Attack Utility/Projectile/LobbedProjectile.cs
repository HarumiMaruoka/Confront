﻿using Confront.Audio;
using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    // 投げるような軌道のプロジェクタイル
    public class LobbedProjectile : ProjectileBase
    {
        [Header("Projectile")]
        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private float _minDuration = 0.5f;
        [SerializeField] private float _maxDuration = 1.0f;
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

        [Header("SFX")]
        [SerializeField] private AudioClip _launchSFX;
        [SerializeField] private AudioClip _hitSFX;

        private Collider[] _colliderBuffer = new Collider[4];

        private float _flightDuration = 2.0f;  // 飛行にかける時間（秒）

        private Vector3 _lastPosition;
        private Vector2 _startPosition;
        private Vector2 _initialVelocity;
        private float _elapsedTime = 0f;

        public event Action<LobbedProjectile> OnCompleted;

        public override void Launch()
        {
            AudioManager.PlaySE(_launchSFX);
            this._elapsedTime = 0f;
            this.transform.position = transform.position + _spawnOffset;
            this._lastPosition = transform.position + _spawnOffset;
            this._startPosition = transform.position + _spawnOffset;
            this._flightDuration = UnityEngine.Random.Range(_minDuration, _maxDuration);
            CalculateInitialVelocity();
        }

        private void CalculateInitialVelocity()
        {
            // 水平方向の初速度：等速運動でターゲットに到達するための値
            float vx = (TargetPosition.x - _startPosition.x) / _flightDuration;

            // 垂直方向の初速度は、重力加速度を考慮して計算する
            // 公式: target.y = start.y + v0y * t + (1/2)*g*t^2　より
            float vy = (TargetPosition.y - _startPosition.y - 0.5f * _gravity * _flightDuration * _flightDuration) / _flightDuration;

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
            var layerMask = LayerUtility.GroundLayerMask;
            if (PlayerController.Instance.StateMachine.CurrentState is not GroundDodge) layerMask |= LayerUtility.PlayerLayerMask;

            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliderBuffer, layerMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hitCount; i++)
            {
                var collider = _colliderBuffer[i];
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    var damageValue = _baseDamage + ActorAttackPower * _damageFactor;
                    var sign = Mathf.Sign(transform.position.x - _lastPosition.x);
                    var damageVector = HitBoxBase.CalcDamageVector(_knockbackDirection, _knockbackForce, sign);
                    damageable.TakeDamage(damageValue, damageVector, collider.bounds.center);
                }
            }

            if (hitCount > 0)
            {
                HandleCollision(transform.position);
            }

            _lastPosition = transform.position;
        }

        private void HandleCollision(Vector3 hitPosition)
        {
            transform.position = hitPosition;

            _projectilePS.Stop();
            _projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // ヒットVFXの再生
            _hit.transform.position = hitPosition;
            _hitPS.transform.position = hitPosition;
            _hitPS.transform.parent = null;
            _hitPS.Play();

            // ヒットSFXの再生
            AudioManager.PlaySE(_hitSFX);

            // パーティクルの停止
            foreach (var detachedPrefab in _detached) detachedPrefab.Stop();

            this.enabled = false;
            OnCompleted?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            if (!enabled) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}