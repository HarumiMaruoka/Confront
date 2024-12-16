using Confront.Player;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class Shooter
    {
        public ChargedProjectile ProjectilePrefab;
        public float FireTime;

        private float _prevElapsed;

        public void Update(PlayerController player, float elapsed, float chargeAmount)
        {
            if (_prevElapsed < FireTime && FireTime <= elapsed)
            {
                Fire(player, chargeAmount);
            }

            _prevElapsed = elapsed;
        }

        public void Fire(PlayerController player, float chargeAmount)
        {
            var projectile = GameObject.Instantiate(ProjectilePrefab, player.transform.position, Quaternion.identity);
            projectile.Initialize(player, chargeAmount);
        }
    }
}