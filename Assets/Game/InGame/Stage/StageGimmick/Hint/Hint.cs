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
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        public string Message { set => _text.text = value; }

        public bool IsVisible { get; private set; }
        public bool IsAnimating => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;


        public void Show()
        {
            IsVisible = true;
            _animator.Play("Show");
        }

        public void Hide()
        {
            IsVisible = false;
            _animator.Play("Hide");
        }
    }
}