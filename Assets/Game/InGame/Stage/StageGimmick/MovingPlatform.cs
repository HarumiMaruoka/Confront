using Confront.Player;
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

        private Collider[] _collidersBuffer;

        private void Start()
        {
            _previousPosition = transform.position;
            _collidersBuffer = new Collider[_capacity];
        }

        private void LateUpdate()
        {
            var moveDelta = transform.position - _previousPosition;
            moveDelta.z = 0;

            int hitCount = Physics.OverlapBoxNonAlloc(transform.position + _offset, _halfSize, _collidersBuffer, transform.rotation, _detectableLayer);
            for (int i = 0; i < hitCount; i++)
            {
                if (_collidersBuffer[i].gameObject == PlayerController.Instance.gameObject)
                {
                    //if (PlayerController.Instance.StateMachine.CurrentState is Grounded)
                    //{
                    //    PlayerController.Instance.CharacterController.Move(_moveDelta);
                    //}
                    PlayerController.Instance.MovementParameters.MovingPlatformDelta += moveDelta;
                }
                else
                {
                    _collidersBuffer[i].transform.position += moveDelta;
                }
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