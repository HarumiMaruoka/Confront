using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class HitBox : HitBoxBase
    {
        [SerializeField]
        private float _startTime;
        [SerializeField]
        private float _endTime;
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _damageFactor;

        [SerializeField]
        private Vector2 _knockbackDirection;
        [SerializeField]
        private float _knockbackForce;

        public void Update(Transform center, float attackPower, float sign, float elapsed, LayerMask layerMask)
        {
            if (_startTime <= elapsed && elapsed <= _endTime) // 有効な時間帯
            {
                var position = center.position + center.rotation * _offset;
                var rotation = center.rotation;

                var damageVector = CalcDamageVector(_knockbackDirection, _knockbackForce, sign);

                ProcessHitBox(attackPower, _baseDamage, _damageFactor, layerMask, position, rotation, damageVector);
            }
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