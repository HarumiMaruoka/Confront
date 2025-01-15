using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public abstract class HitSphereBase
    {
        [Header("HitSphere")]
        [SerializeField]
        protected Vector3 _offset;
        [SerializeField]
        protected float _radius;
        [SerializeField]
        protected GizmoOption _gizmoOption;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        protected Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        protected HashSet<int> _alreadyHits = new HashSet<int>();

        public bool IsOverlapping(Transform center, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            return Physics.OverlapSphereNonAlloc(position, _radius, _colliderBuffer, layerMask, QueryTriggerInteraction.Collide) != 0;
        }

        public static Vector2 CalcDamageVector(Vector2 direction, float force, float sign)
        {
            sign = Mathf.Sign(sign);
            direction.x *= sign;
            if (direction.y > 0f) direction.y *= 4;
            return direction.normalized * force;
        }

        protected void ProcessHitSphere(float attackPower, float baseDamage, float factor, LayerMask layerMask, Vector3 position, Vector2 damageVector)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(position, _radius, _colliderBuffer, layerMask, QueryTriggerInteraction.Collide);
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