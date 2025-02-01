using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.Stage.Hint
{
    public class Hint : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private float _fadeDuration = 0.5f;

        private CancellationTokenSource _cancellationTokenSource;

        public async void Hide()
        {
            await Fade(0, 1);
        }

        public async void Show()
        {
            await Fade(1, 0);
        }

        private async UniTask Fade(float from, float to)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _canvasGroup.alpha = from;

            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                if (token.IsCancellationRequested) return;
                if (this == null) return;

                _canvasGroup.alpha = Mathf.Lerp(from, to, t / _fadeDuration);
                await UniTask.Yield();
            }

            _canvasGroup.alpha = to;
        }
    }
}