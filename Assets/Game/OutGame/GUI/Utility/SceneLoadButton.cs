using Cysharp.Threading.Tasks;
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

        private void Start() => GetComponent<Button>().onClick.AddListener(() =>
        {
             SceneManager.LoadScene(_sceneName);
        });
    }
}