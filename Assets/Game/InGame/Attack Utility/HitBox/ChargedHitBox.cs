using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class ChargedHitBox : HitBoxBase
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
        private Vector2 _minKnockbackDirection;
        [SerializeField]
        private Vector2 _maxKnockbackDirection;

        [SerializeField]
        private float _minKnockbackForce;
        [SerializeField]
        private float _maxKnockbackForce;

        public void Update(Transform center, float attackPower, float sign, float elapsed, float chargeAmount, LayerMask layerMask, bool isCameraShake)
        {
            if (_startTime <= elapsed && elapsed <= _endTime) // 有効な時間帯
            {
                var position = center.position + center.rotation * _offset;
                var rotation = center.rotation;
                sign = Mathf.Sign(sign);

                var factor = Mathf.Lerp(_minDamageFactor, _maxDamageFactor, chargeAmount);
                var damageDirection = Vector2.Lerp(_minKnockbackDirection, _maxKnockbackDirection, chargeAmount);
                var damageForce = Mathf.Lerp(_minKnockbackForce, _maxKnockbackForce, chargeAmount);
                var damageVector = CalcDamageVector(damageDirection, damageForce, sign);

                ProcessHitBox(attackPower, _baseDamage, factor, layerMask, position, rotation, damageVector, isCameraShake);
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
}