using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class AttackHitBox : HitBoxBase
    {
        [SerializeField]
        private float _startTime;
        [SerializeField]
        private float _endTime;
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _factor;

        [SerializeField]
        private Vector2 _damageDirection;
        [SerializeField]
        private float _damageForce;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        private HashSet<int> _alreadyHits = new HashSet<int>();
        private Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        public override bool IsOverlapping(Transform center, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            return Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask) != 0;
        }

        public void Update(Transform center, float attackPower, float sign, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;

            var damageVector = CalcDamageVector(_damageDirection, _damageForce, sign);

            if (_startTime <= elapsed && elapsed <= _endTime) // 有効な時間帯
            {
                var hitCount = Physics.OverlapBoxNonAlloc(position, _size * 0.5f, _colliderBuffer, rotation, layerMask);
                for (int i = 0; i < hitCount; i++)
                {
                    var collider = _colliderBuffer[i];
                    var instanceId = collider.gameObject.GetInstanceID();
                    if (!_alreadyHits.Add(instanceId)) continue;
                    if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        var damage = _baseDamage + attackPower * _factor;
                        damageable.TakeDamage(damage, damageVector);
                    }
                }
            }
        }

        public override void Clear()
        {
            _alreadyHits.Clear();
        }

        public override void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
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

    public enum GizmoOption
    {
        None, // 非表示
        AlwaysVisible, // 常に表示
        RuntimeOnlyVisible, // 実行時のみ表示
        RuntimeAndHitDetectionOnlyVisible, // 実行時かつ有効時のみ表示
    }
}