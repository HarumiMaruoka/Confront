using UnityEngine;

namespace Helper
{
    [System.Serializable]
    public class OverLapSphere
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private LayerMask _targetLayer;

        private Transform _origin;

        public Vector3 Offset => _offset;
        public float Radius => _radius;
        public LayerMask TargetLayer => _targetLayer;
        public Transform Origin => _origin;

        private Collider[] _colliders = null;

        public void Init(Transform origin)
        {
            _origin = origin;
        }
        /// <summary>
        /// このクラスを使用する際は, Update()上部でこのメソッドを呼び出してください。
        /// </summary>
        public void Enter()
        {
            _colliders = null;
        }

        public Collider[] GetCollider()
        {
            if (_colliders != null) return _colliders;

            else
            {
                var dir = _origin.rotation * _offset;
                return _colliders = Physics.OverlapSphere(_origin.position + dir, _radius, _targetLayer);
            }
        }

        public bool IsHit()
        {
            return GetCollider().Length > 0;
        }

        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _gizmoHitColor = Color.red;
        [SerializeField]
        private Color _gizmoNotHitColor = Color.blue;

        public void OnDrawGizmos(Transform origin)
        {
            if (_isDrawGizmo)
            {
                var dir = origin.rotation * _offset;

                if (Physics.OverlapSphere(origin.position + dir, _radius, _targetLayer).Length > 0)
                {
                    Gizmos.color = _gizmoHitColor;
                }
                else
                {
                    Gizmos.color = _gizmoNotHitColor;
                }
                Gizmos.DrawSphere(origin.position + dir, _radius);
            }
        }
    }
}