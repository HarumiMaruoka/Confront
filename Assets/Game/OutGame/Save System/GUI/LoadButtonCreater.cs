using Confront.GameUI;
using Confront.Input;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.SaveSystem.GUI
{
    public class LoadButtonCreator : MonoBehaviour
    {
        [SerializeField]
        private LoadButton _loadButtonPrefab;
        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private ScrollToSelected _scrollToSelected;

        private Button[] _buttons;

        private void Awake()
        {
            // ボタンの生成
            var saveDataControllers = SaveDataRepository.SaveDataControllers;
            _buttons = new Button[saveDataControllers.Length];
            for (int i = 0; i < saveDataControllers.Length; i++)
            {
                var loadButton = Instantiate(_loadButtonPrefab, _parent);
                loadButton.SaveDataController = saveDataControllers[i];
                _buttons[i] = loadButton.Button;
            }

            // ナビゲーションの設定
            for (int i = 0; i < _buttons.Length; i++)
            {
                var button = _buttons[i];
                var navigation = button.navigation;
                navigation.mode = Navigation.Mode.Explicit;

                navigation.selectOnUp = i > 0 ? _buttons[i - 1] : button; // i - 1が0より大きい場合は 一つ前の要素を、そうでない場合は自身を設定する。
                navigation.selectOnDown = i < _buttons.Length - 1 ? _buttons[i + 1] : button; // i + 1が 配列の長さより小さい場合は 次の要素を、そうでない場合は自身を設定する。

                navigation.selectOnLeft = button;
                navigation.selectOnRight = button;

                button.navigation = navigation;
            }
        }

        private void Update()
        {
            var selected = EventSystem.current?.currentSelectedGameObject?.GetComponent<RectTransform>();
            if (selected) _scrollToSelected.ScrollTo(selected);
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