using System;
using UnityEngine;

namespace Confront.StageGimmick
{
    public class MovingPlatform3 : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        [SerializeField]
        private Vector3 _halfSize = Vector3.one;
        [SerializeField]
        private LayerMask _detectableLayer = -1;
        [SerializeField]
        [Range(1, 30)]
        private int _capacity = 10;

        private Collider[] _collidersBuffer;

        private void Start()
        {
            _collidersBuffer = new Collider[_capacity];
        }

        private void Update()
        {
            int hitCount = Physics.OverlapBoxNonAlloc(transform.position + _offset, _halfSize, _collidersBuffer, transform.rotation, _detectableLayer);
            for (int i = 0; i < hitCount; i++)
            {
                _collidersBuffer[i].transform.SetParent(transform);
            }
        }

        private void OnDrawGizmos()
        {
            var isHit = Physics.CheckBox(transform.position + _offset, _halfSize, transform.rotation, _detectableLayer);

            Gizmos.color = isHit ? Color.red : Color.green;

            Gizmos.DrawWireCube(transform.position + _offset, _halfSize * 2);
        }
    }
}