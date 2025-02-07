using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class ChargedHitSphere : HitSphereBase
    {
        [SerializeField]
        private float _startTime;
        [SerializeField]
        private float _endTime;
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _minDamageFactor;
        [SerializeField]
        private float _maxDamageFactor;

        [SerializeField]
        private Vector2 _minKnockBackDirection;
        [SerializeField]
        private Vector2 _maxKnockBackDirection;

        [SerializeField]
        private float _minKnockBackForce;
        [SerializeField]
        private float _maxKnockBackForce;

        public void Update(Transform center, float attackPower, float sign, float elapsed, LayerMask layerMask)
        {
            if (_startTime <= elapsed && elapsed <= _endTime) // 有効な時間帯
            {
                var position = center.position + center.rotation * _offset;
                sign = Mathf.Sign(sign);

                var factor = Mathf.Lerp(_minDamageFactor, _maxDamageFactor, attackPower);
                var damageDirection = Vector2.Lerp(_minKnockBackDirection, _maxKnockBackDirection, attackPower);
                var damageForce = Mathf.Lerp(_minKnockBackForce, _maxKnockBackForce, attackPower);
                var damageVector = CalcDamageVector(damageDirection, damageForce, sign);

                ProcessHitSphere(attackPower, _baseDamage, factor, layerMask, position, damageVector);
            }
        }

        public override void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            var isRuntime = Application.isPlaying;
            var isHitSphereEnabled = _startTime <= elapsed && elapsed <= _endTime;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            if (_gizmoOption == GizmoOption.RuntimeAndHitDetectionOnlyVisible && (!isRuntime || !isHitSphereEnabled)) return;
            Gizmos.color = IsOverlapping(center, layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
        }
    }
}