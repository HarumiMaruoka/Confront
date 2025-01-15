using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class HitSphere : HitSphereBase
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

        public void Update(Transform center, float attackPower, float sign, float elapsed, LayerMask layerMask)
        {
            if (_startTime <= elapsed && elapsed <= _endTime) // 有効な時間帯
            {
                var position = center.position + center.rotation * _offset;
                var damageVector = CalcDamageVector(_damageDirection, _damageForce, sign);

                ProcessHitSphere(attackPower, _baseDamage, _factor, layerMask, position, damageVector);
            }
        }

        public override void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var isRuntime = Application.isPlaying;
            var isHitSphereEnabled = _startTime <= elapsed && elapsed <= _endTime;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            if (_gizmoOption == GizmoOption.RuntimeAndHitDetectionOnlyVisible && (!isRuntime || !isHitSphereEnabled)) return;
            Gizmos.color = IsOverlapping(center, layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
        }
    }
}