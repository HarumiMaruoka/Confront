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

        public bool GetResult()
        {
            return Physics.Raycast(_origin.position, _dir, out _result, _maxDistance, _targetLayer);
        }
        public bool GetResult(out RaycastHit result)
        {
            var isHit = Physics.Raycast(_origin.position, _dir, out result, _maxDistance, _targetLayer);
            _result = result;
            return isHit;
        }
    }
}