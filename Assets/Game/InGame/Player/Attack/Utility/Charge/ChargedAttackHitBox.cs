using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class ChargedAttackHitBox
    {
        [SerializeField]
        private float _startTime;
        [SerializeField]
        private float _endTime;
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _minFactor;
        [SerializeField]
        private float _maxFactor;
        [SerializeField]
        private Vector2 _minDamageVector;
        [SerializeField]
        private Vector2 _maxDamageVector;

        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _size;
        [SerializeField]
        private GizmoOption _gizmoOption;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        private HashSet<int> _alreadyHits = new HashSet<int>();
        private Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        //[HideInInspector]
        //public Transform Center;

        //public Vector3 Position => Center.position + Center.rotation * _offset;
        //public Quaternion Rotation => Center.rotation;

        public bool IsOverlapping(Transform center, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            return Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask) != 0;
        }

        public void Update(Transform center, float attackPower, float elapsed, float chargeAmount, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;

            if (_startTime <= elapsed && elapsed <= _endTime)
            {
                // 有効な時間帯
                var factor = Mathf.Lerp(_minFactor, _maxFactor, chargeAmount);
                var damageVector = Vector2.Lerp(_minDamageVector, _maxDamageVector, chargeAmount);
                var hitCount = Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask);
                for (int i = 0; i < hitCount; i++)
                {
                    var collider = _colliderBuffer[i];
                    var instanceId = collider.gameObject.GetInstanceID();
                    if (!_alreadyHits.Add(instanceId)) continue;
                    if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        var damage = _baseDamage + attackPower * factor;
                        damageable.TakeDamage(damage, damageVector);
                    }
                }
            }
        }

        public void Clear()
        {
            _alreadyHits.Clear();
        }

        public void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;

            var isRuntime = Application.isPlaying;
            var isHitBoxEnabled = _startTime <= elapsed && elapsed <= _endTime;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            if (_gizmoOption == GizmoOption.RuntimeAndHitDetectionOnlyVisible && (!isRuntime || !isHitBoxEnabled)) return;
            Gizmos.color = IsOverlapping(center, layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _size);
        }
    }
}