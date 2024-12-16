using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class AttackHitBox
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
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _size;
        [SerializeField]
        private GizmoOption _gizmoOption;

        private const int MAX_COLLIDER_BUFFER_SIZE = 16; // 一度に取得できるコライダーの最大数。
        private HashSet<int> _alreadyHits = new HashSet<int>();
        private Collider[] _colliderBuffer = new Collider[MAX_COLLIDER_BUFFER_SIZE];

        [HideInInspector]
        public Transform Center;

        public Vector3 Position => Center.position + Center.rotation * _offset;
        public Quaternion Rotation => Center.rotation;

        public bool IsOverlapping(LayerMask layerMask) => Physics.OverlapBoxNonAlloc(Position, _size * 0.5f, _colliderBuffer, Rotation, layerMask) != 0;

        public void Update(PlayerController player, float elapsed, LayerMask layerMask)
        {
            if (_startTime <= elapsed && elapsed <= _endTime)
            {
                // 有効な時間帯
                var hitCount = Physics.OverlapBoxNonAlloc(Position, _size * 0.5f, _colliderBuffer, Rotation, layerMask);
                for (int i = 0; i < hitCount; i++)
                {
                    var collider = _colliderBuffer[i];
                    var instanceId = collider.gameObject.GetInstanceID();
                    if (!_alreadyHits.Add(instanceId)) continue;
                    if (collider.gameObject.TryGetComponent(out IDamageable damageable))
                    {
                        var playerAttackPower = player.CharacterStats.AttackPower;
                        var damage = _baseDamage + playerAttackPower * _factor;
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }

        public void Clear()
        {
            _alreadyHits.Clear();
        }

        public void DrawGizmos(float elapsed, LayerMask layerMask)
        {
            var isRuntime = Application.isPlaying;
            var isHitBoxEnabled = _startTime <= elapsed && elapsed <= _endTime;
            if (_gizmoOption == GizmoOption.None) return;
            if (_gizmoOption == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            if (_gizmoOption == GizmoOption.RuntimeAndHitDetectionOnlyVisible && (!isRuntime || !isHitBoxEnabled)) return;
            if (!Center) Center = GameObject.FindGameObjectWithTag("Player").transform;
            Gizmos.color = IsOverlapping(layerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(Position, Rotation, Vector3.one);
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