using Confront.Input;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Stage.Hint
{
    [DefaultExecutionOrder(-100)] // プレイヤーよりも先に実行されるようにする
    public class HintActivator : MonoBehaviour
    {
        [SerializeField]
        private string _message = "Hello, World!";

        [SerializeField]
        private Vector3 _hitBoxHalfSize = new Vector3(3, 1, 1);

        private static LayerMask _layerMask = -1;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeLayerMask()
        {
            _layerMask = LayerMask.GetMask("Player");
        }

        private void Update()
        {
            var hint = Hint.Instance;
            var hit = Physics.CheckBox(transform.position, _hitBoxHalfSize, Quaternion.identity, _layerMask);
            if (hit && !hint.IsVisible && !hint.IsAnimating)
            {
                hint.Message = _message;
                hint.Show();
            }
            else if (!hit && hint.IsVisible && !hint.IsAnimating)
            {
                hint.Hide();
            }
        }

        private void OnDrawGizmos()
        {
            var hit = Physics.CheckBox(transform.position, _hitBoxHalfSize, Quaternion.identity, _layerMask);
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawWireCube(transform.position, _hitBoxHalfSize * 2);
        }
    }
}