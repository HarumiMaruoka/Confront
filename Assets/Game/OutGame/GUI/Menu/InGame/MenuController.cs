using Confront.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.GameUI
{
    // インゲーム用のメニューUIを制御するクラス。
    public class MenuController : MonoBehaviour
    {
        public static MenuController Instance { get; private set; }
        public static bool IsOpened => Instance.IsOpenedMenu;
        public static event Action OnOpenedMenu;
        public static event Action OnClosedMenu;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("MenuController is already exist.");
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public Transform Parent;

        [SerializeField]
        private GameObject _mainMenuPrefab;
        [SerializeField]
        private GameObject _saveMenuPrefab;

        private GameObject _mainMenu;
        private GameObject _saveMenu;

        private Stack<GameObject> _menus = new Stack<GameObject>();

        public GameObject CurrentMenu => _menus.Count > 0 ? _menus.Peek() : null;

        public bool IsOpenedMenu => _menus.Count > 0;
        private Player.IState PlayerState => Player.PlayerController.Instance.StateMachine.CurrentState;


        private void Update()
        {
            if (PlayerState is Player.Dead)
            {
                while (_menus.Count > 0) CloseMenu();
                return;
            }

            if (_menus.Count > 0 && PlayerInputHandler.OutGameInput.CloseMenu.triggered)
            {
                CloseMenu();
            }
            else if (_menus.Count == 0 && PlayerInputHandler.OutGameInput.OpenMenu.triggered)
            {
                if (!_mainMenu) _mainMenu = Instantiate(_mainMenuPrefab, Parent);
                OpenMenu(_mainMenu);
            }
            else if (_menus.Count == 0 && PlayerInputHandler.OutGameInput.SaveShortcut.triggered)
            {
                if (!_saveMenu) _saveMenu = Instantiate(_saveMenuPrefab, Parent);
                OpenMenu(_saveMenu);
            }
        }

        public void OpenMenu(GameObject menu)
        {
            if (_menus.Count == 0) OnOpenedMenu?.Invoke();

            menu.SetActive(true);
            menu.transform.SetAsLastSibling();
            _menus.Push(menu);
        }

        public void CloseMenu()
        {
            if (_menus.Count > 0)
            {
                var menu = _menus.Pop();
                menu.gameObject.SetActive(false);
                if (_menus.Count == 0) OnClosedMenu?.Invoke();
            }
        }
    }
}