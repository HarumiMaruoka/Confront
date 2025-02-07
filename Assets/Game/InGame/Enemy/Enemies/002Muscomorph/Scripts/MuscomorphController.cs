using Confront.AttackUtility;
using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy
{
    public class MuscomorphController : SlimeyController
    {
        [SerializeField]
        private LobbedProjectile _bulletPrefab;
        [SerializeField]
        private Vector3 _bulletSpawnOffset = new Vector2(0, 0.5f);
        [SerializeField]
        private Vector3 _targetSpawnOffset = new Vector2(0, 0.5f);
        [SerializeField]
        private float _bulletMinDuration = 2f;
        [SerializeField]
        private float _bulletMaxDuration = 5f;

        private Queue<LobbedProjectile> _inactiveBullets = new Queue<LobbedProjectile>();

        public override void Attack() // アニメーションイベントから呼び出す
        {
            var bullet = GetBullet();
            var bulletDuration = UnityEngine.Random.Range(_bulletMinDuration, _bulletMaxDuration);
            var targetPosition = PlayerController.Instance.transform.position + _targetSpawnOffset;
            bullet.Launch(Stats.AttackPower, transform.position + _bulletSpawnOffset, targetPosition, bulletDuration);
        }

        private LobbedProjectile GetBullet()
        {
            LobbedProjectile instance;
            if (_inactiveBullets.Count > 0)
            {
                instance = _inactiveBullets.Dequeue();
            }
            else
            {
                instance = Instantiate(_bulletPrefab, transform);
            }
            instance.OnCompleted += OnBulletCompleted;
            return instance;
        }

        private void OnBulletCompleted(LobbedProjectile bullet)
        {
            bullet.OnCompleted -= OnBulletCompleted;
            _inactiveBullets.Enqueue(bullet);
        }
    }
}
