using Confront.Audio;
using Confront.CameraUtilites;
using Confront.Player;
using Confront.Utility;
using Confront.VFXSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public abstract class HitBoxBase
    {
        [Header("HitBox")]
        [SerializeField] protected Vector3 _offset;
        [SerializeField] protected Vector3 _size;
        [SerializeField] protected GizmoOption _gizmoOption;

        public VFXParameters _hitVFX;
        public AudioClip _hitSFX;
        public CameraShakeParameters _cameraShake;

        public GizmoOption GizmoOption => _gizmoOption;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        protected Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        protected HashSet<int> _alreadyHits = new HashSet<int>();

        public bool IsOverlapping(Transform center, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            return Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask, QueryTriggerInteraction.Collide) != 0;
        }

        public static Vector2 CalcDamageVector(Vector2 direction, float knockbackForce, float sign)
        {
            sign = Mathf.Sign(sign);
            direction.x *= sign;
            if (direction.y > 0f) direction.y *= 4;
            return direction.normalized * knockbackForce;
        }

        protected bool ProcessHitBox(
            float attackPower,
            float baseDamage,
            float factor,
            LayerMask layerMask,
            Vector3 position,
            Quaternion rotation,
            Vector2 damageVector,
            bool isCameraShake)
        {
            var hitCount = Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask, QueryTriggerInteraction.Collide);
            var isHit = hitCount > 0;

            int successfulHitCount = 0;

            factor = Mathf.Max(1, factor);

            for (int i = 0; i < hitCount; i++)
            {
                var collider = _colliderBuffer[i];
                var instanceId = collider.GetInstanceID();
                if (!_alreadyHits.Add(instanceId)) continue;
                if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                {

                    if (!_alreadyHits.Add(damageable.Owner.GetInstanceID())) continue;

                    var damage = baseDamage + attackPower * factor;
                    damageable.TakeDamage(damage, damageVector, collider.bounds.center);
                    CreateHitVFX(position, collider);
                    successfulHitCount++;
                }
            }

            if (successfulHitCount > 0)
            {
                if (isCameraShake) CameraShakeHandler.Instance.Shake(_cameraShake.Amplitude, _cameraShake.Frequency, _cameraShake.Duration);
                if (_hitSFX) AudioManager.PlaySE(_hitSFX);
                HitStopHandler.Stop(0.25f);
            }

            return hitCount > 0;
        }

        public void Clear()
        {
            _alreadyHits.Clear();
        }

        public void CreateHitVFX(Vector3 hitBoxCenter, Collider hitCollider)
        {
            var weaponPosition = PlayerController.Instance.WeaponActivator.Current.transform.position;
            var colliderCenter = hitCollider.bounds.center;

            var ray = new Ray(weaponPosition, colliderCenter - weaponPosition);
            var layerMask = LayerUtility.EnemyLayerMask | LayerUtility.NoCollisionEnemy | LayerUtility.PlatformEnemy;
            var isHit = Physics.Raycast(ray, out var hitInfo, 30f, layerMask, QueryTriggerInteraction.Ignore);

            var position = isHit ? hitInfo.point + _hitVFX.Offset : weaponPosition + _hitVFX.Offset;
            var rotation = UnityEngine.Random.rotation;
            var size = Vector3.Lerp(_hitVFX.MinSize, _hitVFX.MaxSize, UnityEngine.Random.value);

            // VFX
            if (_hitVFX.VFXPrefab)
            {
                VFXManager.PlayVFX(_hitVFX.VFXPrefab, position, rotation, size);
            }
        }

        public abstract void DrawGizmos(Transform center, float elapsed, LayerMask layerMask);
    }

    [Serializable]
    public class CameraShakeParameters
    {
        public float Amplitude = 2;
        public float Frequency = 1;
        public float Duration = 0.1f;
    }

    [Serializable]
    public class VFXParameters
    {
        public VFX VFXPrefab;
        public Vector3 Offset = new Vector3(0f, 0f, -0.5f);
        public Vector3 MinSize = new Vector3(3, 3, 3);
        public Vector3 MaxSize = new Vector3(4, 4, 4);
    }
}