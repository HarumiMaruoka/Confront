using Confront.GameUI;
using Confront.Input;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Stage.Hint
{
    public class TutorialMessage : MonoBehaviour
    {
        [SerializeField]
        private string _prefixMessage = "Hello, World!";
        [SerializeField]
        private InGameInputSpriteInfo.InputAction _inputAction;
        [SerializeField]
        private string _suffixMessage = " to do something.";

        [SerializeField]
        private Vector2 _hitBoxHalfSize = new Vector2(6, 3);

        private static bool _isPlayerInsideArea = false;
        private static string _message;
        private static int _frameCount = 0;

        private void Update()
        {
            if (_isPlayerInsideArea) return;

            _isPlayerInsideArea = IsPlayerInsideHintArea();
            _message = _prefixMessage + InGameInputSpriteInfo.Instance.GetSprite(_inputAction) + _suffixMessage;
        }

        private void LateUpdate()
        {
            if (_frameCount == Time.frameCount) return;
            _frameCount = Time.frameCount;

            var hint = Hint.Instance;

            if (_isPlayerInsideArea && !hint.IsVisible && !hint.IsAnimating)
            {
                hint.Message = _message;
                hint.Show();
            }
            else if (!_isPlayerInsideArea && hint.IsVisible && !hint.IsAnimating)
            {
                hint.Hide();
            }

            _isPlayerInsideArea = false;
        }

        private bool IsPlayerInsideHintArea()
        {
            var playerPosition = PlayerController.Instance.transform.position;
            var leftTop = transform.position + new Vector3(-_hitBoxHalfSize.x, _hitBoxHalfSize.y, 0);
            var rightBottom = transform.position + new Vector3(_hitBoxHalfSize.x, -_hitBoxHalfSize.y, 0);
            var isPlayerInsideArea =
                playerPosition.x >= leftTop.x && playerPosition.x <= rightBottom.x &&
                playerPosition.y <= leftTop.y && playerPosition.y >= rightBottom.y;
            return isPlayerInsideArea;
        }

        private void OnDrawGizmos()
        {
            var hit = IsPlayerInsideHintArea();
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawWireCube(transform.position, _hitBoxHalfSize * 2);
        }
    }
}