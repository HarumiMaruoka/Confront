using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.SaveSystem.GUI
{
    public class SaveButtonCreator : MonoBehaviour
    {
        [SerializeField]
        private SaveButton _saveButtonPrefab;
        [SerializeField]
        private Transform _saveButtonParent;

        private Button[] _buttons;

        private void Awake()
        {
            // ボタンの生成
            var saveDataControllers = SaveDataRepository.SaveDataControllers;
            _buttons = new Button[saveDataControllers.Length];
            for (int i = 0; i < saveDataControllers.Length; i++)
            {
                var saveButton = Instantiate(_saveButtonPrefab, _saveButtonParent);
                saveButton.SaveDataController = saveDataControllers[i];
                _buttons[i] = saveButton.Button;
            }

            // ナビゲーションの設定
            for (int i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];
                var navigation = button.navigation;
                navigation.mode = Navigation.Mode.Explicit;

                navigation.selectOnUp = i > 0 ? _buttons[i - 1] : button; // 三項演算子を使って、iが0より大きい場合は_buttons[i - 1]を、そうでない場合は自身を代入する。
                navigation.selectOnDown = i < _buttons.Length - 1 ? _buttons[i + 1] : button; // 三項演算子を使って、iが_buttons.Length - 1より小さい場合は_buttons[i + 1]を、そうでない場合は自身を代入する。

                button.navigation = navigation;
            }
        }

        private GameObject _previouseSelection;

        private void OnEnable()
        {
            _previouseSelection = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
        }

        private void OnDisable()
        {
            if (_previouseSelection)
            {
                EventSystem.current?.SetSelectedGameObject(_previouseSelection);
            }
            else
            {
                Debug.LogWarning("Previous selection is null");
            }
        }
    }
}