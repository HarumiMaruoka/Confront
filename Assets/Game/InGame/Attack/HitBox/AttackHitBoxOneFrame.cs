using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class AttackHitBoxOneFrame : HitBoxBase
    {
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _factor;

        [SerializeField]
        private Vector2 _damageDirection;
        [SerializeField]
        private float _damageForce;

        public void Fire(Transform center, float sign, float attackPower, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            sign = Mathf.Sign(sign);

            var damageVector = CalcDamageVector(_damageDirection, _damageForce, sign);

            ProcessHitBox(attackPower, _baseDamage, _factor, layerMask, position, rotation, damageVector);
        }

        public override void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            var isRuntime = Application.isPlaying;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            Gizmos.color = IsOverlapping(center, layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _size);
        }
    }
}