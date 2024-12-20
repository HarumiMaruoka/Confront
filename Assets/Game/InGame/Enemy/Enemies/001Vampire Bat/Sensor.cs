using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [Serializable]
    public class Sensor
    {
        [SerializeField]
        private Vector3 _groundCheckOffset = new Vector2(0, -0.5f);
        [SerializeField]
        private float _groundCheckDistance = 0.1f;
        [SerializeField]
        private LayerMask _groundLayerMask;
        [SerializeField]
        private bool _isDrawGizmos;

        public bool IsGrounded(Transform transform, out RaycastHit hit)
        {
            Vector3 offset = transform.rotation * _groundCheckOffset;
            return Physics.Raycast(transform.position + offset, Vector3.down, out hit, _groundCheckDistance, _groundLayerMask);
        }

        public void DrawGizmos(Transform transform)
        {
            if (!_isDrawGizmos) return;

            Vector3 offset = transform.rotation * _groundCheckOffset;
            var isGrounded = IsGrounded(transform, out RaycastHit hit);
            Gizmos.color = isGrounded ? Color.red : Color.blue;
            if (isGrounded)
            {
                Gizmos.DrawRay(transform.position + offset, Vector3.down * hit.distance);
            }
            else
            {
                Gizmos.DrawRay(transform.position + offset, Vector3.down * _groundCheckDistance);
            }
        }
    }
}