using System;
using UnityEngine;

namespace Confront.Enemy.Test
{
    public class TestPlayerDamageBulletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float interval = 1f;
        [SerializeField] private bool _spawning = true;

        private Renderer _renderer;
        private Color _matCol;

        private float _timer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _matCol = _renderer.material.color;
            UpdateMaterialColor();
        }

        private void Update()
        {
            if (!_spawning) return;

            _timer += Time.deltaTime;
            if (_timer >= interval)
            {
                _timer = 0;
                Instantiate(bulletPrefab, transform.position + _offset, transform.rotation);
            }
        }

        private void OnMouseDown()
        {
            _spawning = !_spawning;
            UpdateMaterialColor();
        }

        private void UpdateMaterialColor()
        {
            if (_spawning)
            {
                _renderer.material.color = _matCol;
            }
            else
            {
                _renderer.material.color = Color.gray;
            }
        }
    }
}