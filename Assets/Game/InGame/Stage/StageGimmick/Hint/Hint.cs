using System;
using UnityEngine;

namespace Confront.Stage.Hint
{
    public class Hint : MonoBehaviour
    {
        public static Hint Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There are multiple Hint instances.");
            }
            Instance = this;
            StageManager.OnStageChanged += OnStageChanged;
        }

        private void OnDestroy()
        {
            Instance = null;
            StageManager.OnStageChanged -= OnStageChanged;
        }

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        public string Message { set => _text.text = value; }

        public bool IsVisible { get; private set; }
        public bool IsAnimating => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f;

        public void Show()
        {
            IsVisible = true;
            _animator.CrossFade("Show", 0.1f);
        }

        public void Hide()
        {
            IsVisible = false;
            _animator.CrossFade("Hide", 0.1f);
        }

        private void OnStageChanged(StageController _)
        {
            Hide();
        }
    }
}