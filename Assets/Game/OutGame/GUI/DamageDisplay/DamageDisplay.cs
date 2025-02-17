using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.GameUI
{
    public class DamageDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        [SerializeField]
        private float _randomX = 1;
        [SerializeField]
        private float _randomY = 1;

        [SerializeField]
        private AnimationCurve _sizeCurve;
        [SerializeField]
        private Gradient _colorGradient;

        public event Action<DamageDisplay> OnHide;

        public async void Show(int damage)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(-_randomX, _randomX), UnityEngine.Random.Range(-_randomY, _randomY), 0);

            var duration = _sizeCurve.keys[_sizeCurve.length - 1].time;

            _text.text = damage.ToString();
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                if (!gameObject) return;

                var size = _sizeCurve.Evaluate(t);
                var color = _colorGradient.Evaluate(t / duration);

                _text.transform.localScale = new Vector3(size, size, size);
                _text.color = color;

                await UniTask.Yield();
            }
            OnHide?.Invoke(this);
        }
    }
}