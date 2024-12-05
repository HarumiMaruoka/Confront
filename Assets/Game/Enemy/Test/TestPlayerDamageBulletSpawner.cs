using System;
using UnityEngine;

namespace Confront.Enemy.Test
{
    public class TestPlayerDamageBulletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float interval = 1f;

        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= interval)
            {
                _timer = 0;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
    }
}