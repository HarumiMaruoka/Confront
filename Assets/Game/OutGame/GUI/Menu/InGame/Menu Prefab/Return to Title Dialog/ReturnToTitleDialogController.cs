using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GameUI
{
    public class ReturnToTitleDialogController : MonoBehaviour
    {
        [SerializeField]
        private Button _yesButton;
        [SerializeField]
        private Button _cancelButton;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private string _openAnimationName;
        [SerializeField]
        private string _closeAnimationName;

        private void Awake()
        {
            _yesButton.onClick.AddListener(() =>
            {
                LoadTitleScene();
            });
            _cancelButton.onClick.AddListener(() =>
            {
                Close();
            });
        }

        private void OnEnable()
        {
            _animator.Play(_openAnimationName);
        }

        private async void LoadTitleScene()
        {
            await ScreenFader.Instance.FadeOut();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        private async void Close()
        {
            _animator.Play(_closeAnimationName);
            await UniTask.Yield();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                if (!this) return;
                await UniTask.Yield();
            }
            MenuController.Instance.CloseMenu();
        }
    }
}