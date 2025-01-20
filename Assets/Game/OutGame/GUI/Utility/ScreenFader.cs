using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GameUI
{
    public class ScreenFader : MonoBehaviour
    {
        private static ScreenFader _instance;
        public static ScreenFader Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        [SerializeField]
        private Image Image;
        [SerializeField]
        private float DefaultFadeDuration = 0.5f;

        private CancellationTokenSource _cancellationTokenSource;

        public UniTask FadeIn()
        {
            return Fade(1, 0, DefaultFadeDuration);
        }

        public UniTask FadeIn(float duration)
        {
            return Fade(1, 0, duration);
        }

        public UniTask FadeOut()
        {
            return Fade(0, 1, DefaultFadeDuration);
        }

        public UniTask FadeOut(float duration)
        {
            return Fade(0, 1, duration);
        }

        private async UniTask Fade(float from, float to, float duration)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            var col = Image.color;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (token.IsCancellationRequested) return;
                col = Image.color;
                Image.color = new Color(col.r, col.g, col.b, Mathf.Lerp(from, to, t / duration));
                await UniTask.Yield();
            }

            col = Image.color;
            Image.color = new Color(col.r, col.g, col.b, to);
        }
    }
}