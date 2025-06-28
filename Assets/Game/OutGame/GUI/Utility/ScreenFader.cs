using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class ScreenFader : MonoBehaviour
    {
        private static ScreenFader _instance;
        public static ScreenFader Instance
        {
            get
            {
                if (!_instance) _instance = CreateScreenFader();
                return _instance;
            }
        }

        private static ScreenFader CreateScreenFader()
        {
            var go = new GameObject("ScreenFader");
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            var fader = go.AddComponent<ScreenFader>();
            fader._image = go.AddComponent<Image>();
            fader._image.rectTransform.anchorMin = Vector2.zero;
            fader._image.rectTransform.anchorMax = Vector2.one;
            fader._image.rectTransform.sizeDelta = Vector2.zero;
            fader._image.color = Color.clear;
            fader._image.raycastTarget = false;
            return fader;
        }

        [SerializeField]
        private Image _image;

        private readonly float _defaultFadeDuration = 0.5f;
        private readonly EaseType DefaultEaseType = EaseType.Linear;

        private CancellationTokenSource _cancellationTokenSource;

        public UniTask FadeIn()
        {
            return Fade(1f, 0f, _defaultFadeDuration, DefaultEaseType);
        }

        public UniTask FadeIn(float duration, EaseType easeType = EaseType.Linear)
        {
            return Fade(1f, 0f, duration, easeType);
        }

        public UniTask FadeOut()
        {
            return Fade(0f, 1f, _defaultFadeDuration, DefaultEaseType);
        }

        public UniTask FadeOut(float duration, EaseType easeType = EaseType.Linear)
        {
            return Fade(0f, 1f, duration, easeType);
        }

        private async UniTask Fade(float from, float to, float duration, EaseType easeType)
        {
            // もし既にフェード中ならキャンセル
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // ループ開始時の色を取得
            var col = _image.color;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (token.IsCancellationRequested) return;

                // 進捗度合い(0~1)
                float progress = t / duration;

                // Easing された進捗度合いを計算
                float easedProgress = Ease(easeType, progress);

                // easedProgress を使ってアルファ値を補間
                float alpha = Mathf.Lerp(from, to, easedProgress);

                // 色を更新
                col = _image.color;
                _image.color = new Color(col.r, col.g, col.b, alpha);

                await UniTask.Yield();
            }

            // 最終的に to にあわせてアルファ値を固定
            col = _image.color;
            _image.color = new Color(col.r, col.g, col.b, to);
        }

        /// <summary>
        /// イージングの種類
        /// </summary>
        public enum EaseType
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce,
        }

        /// <summary>
        /// EaseType に応じた Easing 関数
        /// </summary>
        private float Ease(EaseType easeType, float t)
        {
            switch (easeType)
            {
                //--------------------------------------------------------------------------------
                // まずはシンプルなもの
                //--------------------------------------------------------------------------------
                default:
                case EaseType.Linear:
                    // リニア: そのまま t を返す
                    return t;

                case EaseType.EaseIn:
                    // 2次関数を例に。いわゆる EaseInQuad
                    // t^2
                    return t * t;

                case EaseType.EaseOut:
                    // EaseOutQuad
                    // 1 - (1 - t)^2
                    return 1f - (1f - t) * (1f - t);

                case EaseType.EaseInOut:
                    // EaseInOutQuad
                    // 前半 = 2t^2, 後半 = -1 + (4 - 2t)*t
                    if (t < 0.5f)
                        return 2f * t * t;
                    else
                        return -1f + (4f - 2f * t) * t;

                //--------------------------------------------------------------------------------
                // Cubic
                //--------------------------------------------------------------------------------
                case EaseType.EaseInCubic:
                    // t^3
                    return t * t * t;

                case EaseType.EaseOutCubic:
                    // 1 - (1 - t)^3
                    return 1f - Mathf.Pow(1f - t, 3f);

                case EaseType.EaseInOutCubic:
                    // 前半 = 4t^3, 後半 = 1 - (-2t + 2)^3 / 2
                    if (t < 0.5f)
                        return 4f * t * t * t;
                    else
                        return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;

                //--------------------------------------------------------------------------------
                // Quint
                //--------------------------------------------------------------------------------
                case EaseType.EaseInQuint:
                    // t^5
                    return Mathf.Pow(t, 5f);

                case EaseType.EaseOutQuint:
                    // 1 - (1 - t)^5
                    return 1f - Mathf.Pow(1f - t, 5f);

                case EaseType.EaseInOutQuint:
                    // 前半 = 16t^5, 後半 = 1 - (-2t + 2)^5 / 2
                    if (t < 0.5f)
                        return 16f * Mathf.Pow(t, 5f);
                    else
                        return 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f;

                //--------------------------------------------------------------------------------
                // Circ
                //--------------------------------------------------------------------------------
                case EaseType.EaseInCirc:
                    // 1 - sqrt(1 - t^2)
                    return 1f - Mathf.Sqrt(1f - t * t);

                case EaseType.EaseOutCirc:
                    // sqrt(1 - (t - 1)^2)
                    return Mathf.Sqrt(1f - (t - 1f) * (t - 1f));

                case EaseType.EaseInOutCirc:
                    if (t < 0.5f)
                        return (1f - Mathf.Sqrt(1f - 4f * t * t)) * 0.5f;
                    else
                        return (Mathf.Sqrt(1f - (2f * t - 1f) * (2f * t - 1f)) + 1f) * 0.5f;

                //--------------------------------------------------------------------------------
                // Elastic や Back, Bounce はより複雑な式なのでお好みで実装
                // 以下は一例
                //--------------------------------------------------------------------------------
                case EaseType.EaseInElastic:
                    {
                        float c4 = (2f * Mathf.PI) / 3f;
                        return t == 0
                            ? 0f
                            : t == 1
                                ? 1f
                                : -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * c4);
                    }

                case EaseType.EaseOutElastic:
                    {
                        float c4 = (2f * Mathf.PI) / 3f;
                        return t == 0
                            ? 0f
                            : t == 1
                                ? 1f
                                : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
                    }

                case EaseType.EaseInOutElastic:
                    {
                        float c5 = (2f * Mathf.PI) / 4.5f;
                        if (t == 0) return 0f;
                        if (t == 1) return 1f;
                        if (t < 0.5f)
                        {
                            return -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) / 2f;
                        }
                        else
                        {
                            return (Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * c5)) / 2f + 1f;
                        }
                    }

                case EaseType.EaseInBack:
                    {
                        const float c1 = 1.70158f;
                        return (c1 + 1f) * t * t * t - c1 * t * t;
                    }

                case EaseType.EaseOutBack:
                    {
                        const float c1 = 1.70158f;
                        float n1 = t - 1f;
                        return 1f + (c1 + 1f) * (n1) * n1 * n1 + c1 * (n1) * n1;
                    }

                case EaseType.EaseInOutBack:
                    {
                        const float c1 = 1.70158f * 1.525f;
                        if (t < 0.5f)
                        {
                            return (Mathf.Pow(2f * t, 2f) * ((c1 + 1f) * 2f * t - c1)) / 2f;
                        }
                        else
                        {
                            float f = 2f * t - 2f;
                            return (Mathf.Pow(f, 2f) * ((c1 + 1f) * f + c1) + 2f) / 2f;
                        }
                    }

                case EaseType.EaseInBounce:
                    // EaseOutBounce を使いまわして実現
                    return 1f - Ease(EaseType.EaseOutBounce, 1f - t);

                case EaseType.EaseOutBounce:
                    {
                        const float n1 = 7.5625f;
                        const float d1 = 2.75f;
                        if (t < 1f / d1)
                        {
                            return n1 * t * t;
                        }
                        else if (t < 2f / d1)
                        {
                            float p = t - 1.5f / d1;
                            return n1 * p * p + 0.75f;
                        }
                        else if (t < 2.5f / d1)
                        {
                            float p = t - 2.25f / d1;
                            return n1 * p * p + 0.9375f;
                        }
                        else
                        {
                            float p = t - 2.625f / d1;
                            return n1 * p * p + 0.984375f;
                        }
                    }

                case EaseType.EaseInOutBounce:
                    if (t < 0.5f)
                    {
                        return (1f - Ease(EaseType.EaseOutBounce, 1f - 2f * t)) * 0.5f;
                    }
                    else
                    {
                        return Ease(EaseType.EaseOutBounce, 2f * t - 1f) * 0.5f + 0.5f;
                    }
            }
        }
    }
}
