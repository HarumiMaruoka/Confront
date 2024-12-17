using Confront.Player;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class ChargedShooter
    {
        public ChargedProjectile ProjectilePrefab;
        public float FireTime;

        private bool _fired = false;

        public void Reset()
        {
            _fired = false;
        }

        public void Update(PlayerController player, float elapsed, float chargeAmount)
        {
            if (FireTime <= elapsed && !_fired)
            {
                Fire(player, chargeAmount);
                _fired = true;
            }
        }

        public void Fire(PlayerController player, float chargeAmount)
        {
            var projectile = GameObject.Instantiate(ProjectilePrefab, player.transform.position, Quaternion.identity);
            projectile.Initialize(player, chargeAmount);
        }
    }
}