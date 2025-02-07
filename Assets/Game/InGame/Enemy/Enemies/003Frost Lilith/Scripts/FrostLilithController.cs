using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class FrostLilithController : SlimeyController
    {
        [Header("Attack")]
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private Transform _spawnPoint;

        public override void Attack() // Animationイベントから呼び出す。
        {
            Vector3 position;
            if (_spawnPoint) position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, 0f);
            else position = new Vector3(transform.position.x, transform.position.y, 0f);

            Instantiate(_bulletPrefab, position, Quaternion.identity);
        }
    }
}