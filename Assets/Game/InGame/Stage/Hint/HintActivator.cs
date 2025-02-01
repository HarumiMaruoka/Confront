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
        private Hint _hintPrefab;
        [SerializeField]
        private Transform _hintContainer;

        [SerializeField]
        private Vector3 _hitBoxHalfSize = new Vector3(3, 1, 1);

        private Hint _hintInstance = null;

        private static LayerMask _layerMask = -1;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeLayerMask()
        {
            _layerMask = LayerMask.GetMask("Player");
        }

        private void Update()
        {
            var hit = Physics.CheckBox(transform.position, _hitBoxHalfSize, Quaternion.identity, _layerMask);
            if (hit)
            {
                if (!_hintInstance) _hintInstance = Instantiate(_hintPrefab, _hintContainer);
                if (PlayerInputHandler.InGameInput.Interact.triggered)
                {
                    _hintInstance.Show();
                    // ヒントを表示したら攻撃インターバルをリセットして、攻撃が発動しないようにする。
                    PlayerController.Instance.MovementParameters.ResetAttackIntervalTimer();
                }
            }
            else
            {
                if (_hintInstance) _hintInstance.Hide();
            }
        }
    }
}