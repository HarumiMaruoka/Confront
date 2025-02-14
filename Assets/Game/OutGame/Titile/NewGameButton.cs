using Confront.SaveSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Confront.Title
{
    public class NewGameButton : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName = "Game Main";

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SaveDataController.Loaded = null;
            SceneManager.LoadScene(_sceneName);
        }
    }
}