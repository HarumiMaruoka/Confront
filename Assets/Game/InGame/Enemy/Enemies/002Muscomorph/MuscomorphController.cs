using Confront.Enemy.Muscomorph;
using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class MuscomorphController : SlimeyController
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        public override void Attack() // アニメーションイベントから呼び出す
        {
            var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        }
}
}
