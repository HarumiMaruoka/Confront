using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy.Test
{
    public class TestPlayerDamageBulletSpawner : MonoBehaviour
    {
        [SerializeField] private TestPlayerDamageBullet bulletPrefab;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float interval = 1f;
        [SerializeField] private bool _spawning = true;

        private Stack<TestPlayerDamageBullet> _inactives = new Stack<TestPlayerDamageBullet>();

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
                CreateBullet();
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

        public TestPlayerDamageBullet CreateBullet()
        {
            TestPlayerDamageBullet bullet;
            if (_inactives.Count > 0)
            {
                bullet = _inactives.Pop();
                bullet.transform.position = transform.position + _offset;
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Instantiate(bulletPrefab, transform.position + _offset, transform.rotation);
            }

            bullet.OnHit -= ReturnBullet;
            bullet.OnHit += ReturnBullet;

            return bullet;
        }

        public void ReturnBullet(TestPlayerDamageBullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _inactives.Push(bullet);
        }
    }
}