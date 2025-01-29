using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.GameUI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private float Alpha
        {
            get => _canvasGroup.alpha;
            set
            {
                _canvasGroup.alpha = value;
                _canvasGroup.gameObject.SetActive(value > 0);
            }
        }

        private void Awake()
        {
            Alpha = 0;
        }

        private CancellationTokenSource _cancellationTokenSource;

        public async void Show(float duration = 1f)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Alpha = 0;
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                if (token.IsCancellationRequested) return;
                if (!this) return;

                Alpha = (Time.time - startTime) / duration;
                await UniTask.Yield();
            }
            Alpha = 1;
        }

        public async void Hide(float duration = 1f)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Alpha = 1;
            var startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                if (token.IsCancellationRequested) return;
                if (!this) return;
                Alpha = 1 - (Time.time - startTime) / duration;
                await UniTask.Yield();
            }
            Alpha = 0;
        }
    }
}