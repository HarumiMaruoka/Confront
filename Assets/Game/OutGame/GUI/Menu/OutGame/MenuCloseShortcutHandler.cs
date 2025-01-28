using Confront.Input;
using System;
using UnityEngine;

namespace Confront.GameUI
{
    // タイトルやゲームオーバー画面用 メニューを閉じるショートカットをハンドルするクラス
    public class MenuCloseShortcutHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _subMenu; // 子メニュー。このメニューが表示されているときはショートカットを無効にする。

        private bool IsSubMenuActive
        {
            get
            {
                if (_subMenu) return _subMenu.activeSelf;
                return false;
            }
        }

        private void Update()
        {
            if (IsSubMenuActive)
            {
                return;
            }
            if (PlayerInputHandler.OutGameInput.CloseMenu.triggered)
            {
                gameObject.SetActive(false);
            }
        }
    }
}