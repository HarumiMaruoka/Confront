using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.GUI
{
    public static class GraphicExtensions
    {
        public static async UniTask Fade(this UnityEngine.UI.Graphic graphic, float to, float duration, CancellationToken token = default)
        {
            var startColor = graphic.color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, to);
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (token.IsCancellationRequested) break;
                elapsedTime += Time.deltaTime;
                graphic.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                await UniTask.Yield(token);
            }
            graphic.color = endColor;
        }

        public static async UniTask FadeIn(this UnityEngine.UI.Graphic graphic, float duration, CancellationToken token = default)
        {
            await Fade(graphic, 1f, duration, token);
        }

        public static async UniTask FadeOut(this UnityEngine.UI.Graphic graphic, float duration, CancellationToken token = default)
        {
            await Fade(graphic, 0f, duration, token);
        }
    }
}