using System;
using UnityEngine;

namespace Helper
{
    [System.Serializable]
    public class Raycast
    {
        [SerializeField]
        private Vector3 _dir = default;
        [SerializeField]
        private float _maxDistance = default;
        [SerializeField]
        private LayerMask _targetLayer = default;

        private Transform _origin = null;
        private RaycastHit _result = default;

        public Vector3 Dir => _dir;
        public float MaxDistance => _maxDistance;
        public LayerMask TargetLayer => _targetLayer;
        public RaycastHit Result => _result;

        public void Init(Transform origin)
        {
            _origin = origin;
        }

        public bool IsHit()
        {
            return Physics.Raycast(_origin.position, _origin.rotation * _dir, out _result, _maxDistance, _targetLayer);
        }
        public bool IsHit(out RaycastHit result)
        {
            var isHit = Physics.Raycast(_origin.position, _origin.rotation * _dir, out result, _maxDistance, _targetLayer);
            _result = result;
            return isHit;
        }
        [SerializeField]
        private bool _isDrawGizmo = true;
        [SerializeField]
        private Color _hitColor = Color.red;
        [SerializeField]
        private Color _noHitColor = Color.blue;
        public void OnDrawGizmo(Transform origin)
        {
            if (_isDrawGizmo)
            {
                // Rayがヒットした場合で色を変える。
                RaycastHit hit;
                if (Physics.Raycast(origin.position, origin.rotation * _dir, out hit, _maxDistance, _targetLayer))
                {
                    //衝突時のRayを画面に表示
                    Debug.DrawRay(
                        origin.position, // 開始位置
                        hit.point - origin.position, //Rayの方向と距離
                        _hitColor, // ヒットした場合の色
                        0, // ラインを表示する時間（秒単位）
                        false); // ラインがカメラから近いオブジェクトによって隠された場合にラインを隠すかどうか
                    return;
                }

                //非衝突時のRayを画面に表示
                Debug.DrawRay(
                    origin.position,
                   (origin.rotation * _dir).normalized * _maxDistance,
                    _noHitColor,
                    0,
                    false);
            }
        }
    }
}