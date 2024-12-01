using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Utility
{
    [Serializable]
    public class AttackHitBox
    {
        [HideInInspector]
        public Transform Center;
        public Vector3 Offset;
        public Vector3 Size;

        public Vector3 Position => Center.position + Center.rotation * Offset;
        public Quaternion Rotation => Center.rotation;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        private HashSet<int> _alreadyHits = new HashSet<int>(); // すでにヒットしたゲームオブジェクトのインスタンスIDを保持する。ヒット判定を一度だけ行うために使用する。
        private Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        public bool IsOverlapping(LayerMask layerMask) => Physics.OverlapBoxNonAlloc(Position, Size * 0.5f, _colliderBuffer, Rotation, layerMask) != 0;

        public void Update(float attackPower, LayerMask layerMask)
        {
            var hitCount = Physics.OverlapBoxNonAlloc(Position, Size * 0.5f, _colliderBuffer, Rotation, layerMask);
            for (var i = 0; i < hitCount; i++)
            {
                var collider = _colliderBuffer[i];
                var instanceId = collider.gameObject.GetInstanceID();
                if (!_alreadyHits.Add(instanceId)) continue;

                if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(attackPower);
                }
            }
        }

        public void Clear()
        {
            _alreadyHits.Clear();
        }
    }
}