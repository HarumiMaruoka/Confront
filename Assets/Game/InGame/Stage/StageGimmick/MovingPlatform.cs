using System;
using UnityEngine;

namespace Confront.StageGimmick
{
    public class MovingPlatform : MonoBehaviour
    {
        private Vector3 _previousPosition;

        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        [SerializeField]
        private Vector3 _halfSize = Vector3.one;
        [SerializeField]
        private LayerMask _detectableLayer = -1;
        [SerializeField]
        [Range(1, 30)]
        private int _capacity = 10;

        private Collider[] _platformColliders;

        private void Start()
        {
            _previousPosition = transform.position;
            _platformColliders = new Collider[_capacity];
        }

        private void Update()
        {
            var moveDelta = transform.position - _previousPosition;

            int hitCount = Physics.OverlapBoxNonAlloc(transform.position + _offset, _halfSize, _platformColliders, transform.rotation, _detectableLayer);
            for (int i = 0; i < hitCount; i++)
            {
                var platform = _platformColliders[i].transform;
                platform.position += moveDelta;
            }

            _previousPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            var isHit = Physics.CheckBox(transform.position + _offset, _halfSize, transform.rotation, _detectableLayer);

            Gizmos.color = isHit ? Color.red : Color.green;

            Gizmos.DrawWireCube(transform.position + _offset, _halfSize * 2);
        }
    }
}