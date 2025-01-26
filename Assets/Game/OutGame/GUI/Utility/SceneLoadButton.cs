using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Confront.GameUI
{
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;

        // ゲームを新規開始する
        private void Start() => GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(_sceneName));
    }
}