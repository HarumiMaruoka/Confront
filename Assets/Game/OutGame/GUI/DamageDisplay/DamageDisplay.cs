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
        private float _duration;

        [SerializeField]
        private float _randomX;
        [SerializeField]
        private float _randomY;

        public event Action<DamageDisplay> OnHide;

        public async void Show(int damage)
        {
            transform.position += new Vector3(UnityEngine.Random.Range(-_randomX, _randomX), UnityEngine.Random.Range(-_randomY, _randomY), 0);

            _text.text = damage.ToString();
            for (float t = 0; t < _duration; t += Time.deltaTime)
            {
                if (!gameObject) return;
                transform.position += Vector3.up * Time.deltaTime;
                await UniTask.Yield();
            }
            OnHide?.Invoke(this);
        }
    }
}