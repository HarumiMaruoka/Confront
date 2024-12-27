using System;
using UnityEngine;

namespace Confront.Debugger
{
    public class TestRandomItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _randomItems;
        [SerializeField]
        private float _interval = 0.1f;
        [SerializeField]
        private Vector3 _spawnOffset;

        private float _timer = 0f;

        private bool _isMouseDown = false;

        private void OnMouseDown()
        {
            _isMouseDown = true;
            _timer = _interval;
        }

        private void Update()
        {
            if (!_isMouseDown) return;

            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0f;
                Spawn();
            }
        }

        private void OnMouseUp()
        {
            _isMouseDown = false;
        }

        private void Spawn()
        {
            var randomOffset = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0f);
            var position = transform.position + randomOffset + _spawnOffset;
            var randomItem = _randomItems[UnityEngine.Random.Range(0, _randomItems.Length)];
            Instantiate(randomItem, position, Quaternion.identity);
        }
    }
}