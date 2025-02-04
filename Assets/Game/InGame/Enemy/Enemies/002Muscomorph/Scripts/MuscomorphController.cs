using Confront.Enemy.Muscomorph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy
{
    public class MuscomorphController : SlimeyController
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        private HashSet<Bullet> _inactiveBullets = new HashSet<Bullet>();

        public override void Attack() // アニメーションイベントから呼び出す
        {
            var bullet = GetBullet();
            bullet.Initialize(transform.position, Stats.AttackPower);
        }

        private Bullet GetBullet()
        {
            Bullet instance;
            if (_inactiveBullets.Count > 0)
            {
                instance = _inactiveBullets.GetEnumerator().Current;
                _inactiveBullets.Remove(instance);
            }
            else
            {
                instance = Instantiate(_bulletPrefab, transform);
            }
            instance.OnCompleted += OnBulletCompleted;
            return instance;
        }

        private void OnBulletCompleted(Bullet bullet)
        {
            bullet.OnCompleted -= OnBulletCompleted;
            _inactiveBullets.Add(bullet);
        }
    }
}
