using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GameUI
{
    public class OpenMenuButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject _menuPrefab;

        private GameObject _menu;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenMenu);
        }

        public void OpenMenu()
        {
            if (!_menuPrefab)
            {
                Debug.LogError("Menu prefab is not assigned.");
                return;
            }

            if (!_menu) _menu = Instantiate(_menuPrefab, MenuController.Instance.Parent);
            MenuController.Instance.OpenMenu(_menu);
        }
    }
}