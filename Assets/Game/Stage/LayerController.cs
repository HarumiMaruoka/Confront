using System;
using UnityEngine;

namespace Confront.Stage
{
    public class LayerController : MonoBehaviour
    {
        [Header("通常時のレイヤー")]
        [SerializeField] private string _defaultLayerName;
        [Header("すり抜け時のレイヤー")]
        [SerializeField] private string _ignoreCollisionLayerName;
        [Header("すり抜け可能なゾーンのレイヤー")]
        [SerializeField] private LayerMask _ignoreCollisionZoneLayer;

        private int _defaultLayer;
        private int _ignoreCollisionLayer;

        private void Start()
        {
            _defaultLayer = LayerMask.NameToLayer(_defaultLayerName);
            _ignoreCollisionLayer = LayerMask.NameToLayer(_ignoreCollisionLayerName);
            _characterController = GetComponent<CharacterController>();
        }

        private CharacterController _characterController;
        private Collider[] _colliderBuffer = new Collider[1];

        private bool _isInColliderDisableZone;
        public bool IsInColliderDisableZone => _isInColliderDisableZone;

        private void Update()
        {
            _isInColliderDisableZone = IsCollisionIgnored();

            if (_isInColliderDisableZone)
            {
                gameObject.layer = _ignoreCollisionLayer;
            }
            else
            {
                gameObject.layer = _defaultLayer;
            }
        }

        private bool IsCollisionIgnored()
        {
            var hitCount = UnityEngine.Physics.OverlapCapsuleNonAlloc(
                _characterController.bounds.center + Vector3.up * (_characterController.height / 2) + Vector3.down * _characterController.radius / 2f,
                _characterController.bounds.center + Vector3.down * (_characterController.height / 2) * 1.2f,
                _characterController.radius * 1.1f,
                _colliderBuffer,
                _ignoreCollisionZoneLayer,
                QueryTriggerInteraction.Collide
            );
            var isIgnoreCollision = hitCount > 0;
            return isIgnoreCollision;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_characterController == null)
            {
                if (!TryGetComponent(out _characterController)) return;
            }

            Gizmos.color = IsCollisionIgnored() ? Color.red : Color.green;

            Vector3 point1 = _characterController.bounds.center + Vector3.up * (_characterController.height / 2) + Vector3.down * _characterController.radius / 2f;
            Vector3 point2 = _characterController.bounds.center + Vector3.down * (_characterController.height / 2) * 1.2f;
            float radius = _characterController.radius * 1.1f;

            Gizmos.DrawWireSphere(point1, radius);
            Gizmos.DrawWireSphere(point2, radius);
            Gizmos.DrawLine(point1, point2);
        }
#endif
    }
}