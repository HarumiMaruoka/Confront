using Confront.Player;
using UnityEngine;

namespace Confront.Stage.Hint
{
    public class HintActivator : MonoBehaviour
    {
        [SerializeField]
        private string _message = "Hello, World!";

        [SerializeField]
        private Vector2 _hitBoxHalfSize = new Vector2(3, 1);

        private void Update()
        {
            var hint = Hint.Instance;

            bool isPlayerInsideArea = IsPlayerInsideHintArea();

            if (isPlayerInsideArea && !hint.IsVisible && !hint.IsAnimating)
            {
                hint.Message = _message;
                hint.Show();
            }
            else if (!isPlayerInsideArea && hint.IsVisible && !hint.IsAnimating)
            {
                hint.Hide();
            }
        }

        private bool IsPlayerInsideHintArea()
        {
            var playerPosition = PlayerController.Instance.transform.position;
            var leftTop = transform.position + new Vector3(-_hitBoxHalfSize.x, _hitBoxHalfSize.y, 0);
            var rightBottom = transform.position + new Vector3(_hitBoxHalfSize.x, -_hitBoxHalfSize.y, 0);
            var isPlayerInsideArea = playerPosition.x >= leftTop.x && playerPosition.x <= rightBottom.x &&
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