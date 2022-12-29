using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    [System.Serializable]
    public class OverLapBox
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _size;
        [SerializeField]
        private LayerMask _targetLayer;

        private Transform _origin;

        public Vector3 Offset => _offset;
        public Vector3 Size => _size;
        public LayerMask TargetLayer => _targetLayer;
        public Transform Origin => _origin;

        public void Init(Transform origin)
        {
            _origin = origin;
        }

        public Collider[] GetCollider()
        {
            var dir = _origin.rotation * _offset; // 回転を考慮した本当のオフセットを取得
            return Physics.OverlapBox(_origin.position + dir, _size / 2f, _origin.rotation, _targetLayer);
        }

        public bool IsHit()
        {
            return GetCollider().Length > 0;
        }

        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _gizmoColor = Color.red;
        public void OnDrawGizmos(Transform origin)
        {
            if (_isDrawGizmo)
            {
                Gizmos.color = _gizmoColor;

                //Gizmo はワールド座標指定なので、相対座標指定の場合はマトリクス変換で移動する
                Gizmos.matrix = Matrix4x4.TRS(origin.position, origin.rotation, origin.localScale);
                Gizmos.DrawCube(_offset, _size);
                Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
            }
        }
    }
}