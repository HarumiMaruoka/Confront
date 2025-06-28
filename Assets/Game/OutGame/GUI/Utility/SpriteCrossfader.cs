using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class SpriteCrossfader : MonoBehaviour
    {
        [SerializeField] private Image front;
        [SerializeField] private Image back;

        public float speed = 10f;

        private CancellationTokenSource cts;

        public async void SetSprite(Sprite sprite)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            var token = cts.Token;

            back.sprite = front.sprite;
            back.color = new Color(1, 1, 1, 1);
            front.sprite = sprite;
            front.color = new Color(1, 1, 1, 0);

            try
            {
                while (front.color.a < 1)
                {
                    if (token.IsCancellationRequested) return;
                    var alpha = front.color.a + Time.deltaTime * speed;
                    front.color = new Color(1, 1, 1, alpha);
                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}