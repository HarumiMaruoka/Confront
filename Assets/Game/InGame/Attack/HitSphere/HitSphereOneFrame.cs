﻿using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class HitSphereOneFrame : HitSphereBase
    {
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
            var position = center.position + center.rotation * _offset;
            var damageVector = CalcDamageVector(_knockbackDirection, _knockbackForce, sign);

            ProcessHitSphere(attackPower, _baseDamage, _damageFactor, layerMask, position, damageVector);
        }

        public override void DrawGizmos(Transform center, float elapsed, LayerMask layerMask)
        {
            var position = center.position + center.rotation * _offset;
            var isRuntime = Application.isPlaying;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            Gizmos.color = IsOverlapping(center, layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireSphere(Vector3.zero, _radius);
        }
    }
}