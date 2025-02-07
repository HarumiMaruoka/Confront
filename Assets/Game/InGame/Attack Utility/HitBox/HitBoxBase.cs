using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public abstract class HitBoxBase
    {
        [Header("HitBox")]
        [SerializeField]
        protected Vector3 _offset;
        [SerializeField]
        protected Vector3 _size;
        [SerializeField]
        protected GizmoOption _gizmoOption;

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

        protected void ProcessHitBox(float attackPower, float baseDamage, float factor, LayerMask layerMask, Vector3 position, Quaternion rotation, Vector2 damageVector)
        {
            var hitCount = Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hitCount; i++)
            {
                var collider = _colliderBuffer[i];
                var instanceId = collider.gameObject.GetInstanceID();
                if (!_alreadyHits.Add(instanceId)) continue;
                if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    var damage = baseDamage + attackPower * factor;
                    damageable.TakeDamage(damage, damageVector);
                }
            }
        }

        public void Clear()
        {
            _alreadyHits.Clear();
        }

        public abstract void DrawGizmos(Transform center, float elapsed, LayerMask layerMask);
    }
}