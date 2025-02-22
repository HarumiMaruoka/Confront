﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class HitBoxOneFrame : HitBoxBase
    {
        [SerializeField]
        private float _baseDamage;
        [SerializeField]
        private float _damageFactor;

        [SerializeField]
        private Vector2 _knockbackDirection;
        [SerializeField]
        private float _knockbackForce;

        public void Fire(Transform center, float sign, float attackPower, LayerMask layerMask, bool isCameraShake)
        {
            var position = center.position + center.rotation * _offset;
            var rotation = center.rotation;
            sign = Mathf.Sign(sign);

            var damageVector = CalcDamageVector(_knockbackDirection, _knockbackForce, sign);

            ProcessHitBox(attackPower, _baseDamage, _damageFactor, layerMask, position, rotation, damageVector, isCameraShake);
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