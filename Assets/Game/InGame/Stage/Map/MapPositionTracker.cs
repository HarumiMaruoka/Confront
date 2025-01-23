using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Stage.Map
{
    [ExecuteAlways]
    public class MapPositionTracker : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _worldLeftTop;
        [SerializeField]
        private Vector2 _worldRightBottom;

        [Space]
        [SerializeField]
        private Vector2 _mapLeftTop;
        [SerializeField]
        private Vector2 _mapRightBottom;

        private void OnValidate()
        {
            if (_worldLeftTop.x > _worldRightBottom.x)
            {
                var temp = _worldLeftTop.x;
                _worldLeftTop.x = _worldRightBottom.x;
                _worldRightBottom.x = temp;
            }
            if (_worldLeftTop.y < _worldRightBottom.y)
            {
                var temp = _worldLeftTop.y;
                _worldLeftTop.y = _worldRightBottom.y;
                _worldRightBottom.y = temp;
            }
        }

        private void Update()
        {
            UpdatePlayerPosition();
        }

        public void UpdatePlayerPosition()
        {
            if (!PlayerController.Instance) return;
            if (PlayerIconHandler.PlayerIconRenderer == null)
            {
                Debug.LogWarning("Player Icon is not set.");
                return;
            }

            var position = PlayerController.Instance.transform.position;
            var mapPosition = ConvertWorldToMapPosition(position);
            PlayerIconHandler.PlayerIconRenderer.transform.position = new Vector3(mapPosition.x, mapPosition.y, 0);
        }

        public Vector2 ConvertWorldToMapPosition(Vector3 position)
        {
            var x = Mathf.InverseLerp(_worldLeftTop.x, _worldRightBottom.x, position.x);
            var y = Mathf.InverseLerp(_worldLeftTop.y, _worldRightBottom.y, position.y);
            var mapX = Mathf.Lerp(_mapLeftTop.x, _mapRightBottom.x, x);
            var mapY = Mathf.Lerp(_mapLeftTop.y, _mapRightBottom.y, y);
            return new Vector2(mapX, mapY);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(_worldLeftTop.x, _worldLeftTop.y), new Vector3(_worldRightBottom.x, _worldLeftTop.y));
            Gizmos.DrawLine(new Vector3(_worldRightBottom.x, _worldLeftTop.y), new Vector3(_worldRightBottom.x, _worldRightBottom.y));
            Gizmos.DrawLine(new Vector3(_worldRightBottom.x, _worldRightBottom.y), new Vector3(_worldLeftTop.x, _worldRightBottom.y));
            Gizmos.DrawLine(new Vector3(_worldLeftTop.x, _worldRightBottom.y), new Vector3(_worldLeftTop.x, _worldLeftTop.y));

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(_mapLeftTop.x, _mapLeftTop.y), new Vector3(_mapRightBottom.x, _mapLeftTop.y));
            Gizmos.DrawLine(new Vector3(_mapRightBottom.x, _mapLeftTop.y), new Vector3(_mapRightBottom.x, _mapRightBottom.y));
            Gizmos.DrawLine(new Vector3(_mapRightBottom.x, _mapRightBottom.y), new Vector3(_mapLeftTop.x, _mapRightBottom.y));
            Gizmos.DrawLine(new Vector3(_mapLeftTop.x, _mapRightBottom.y), new Vector3(_mapLeftTop.x, _mapLeftTop.y));
        }
    }
}