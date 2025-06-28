using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI
{
    [RequireComponent(typeof(Button))]
    public class QuitButton : MonoBehaviour
    {
        private void Start() => GetComponent<Button>().onClick.AddListener(OnButtonClicked);

        private void OnButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}