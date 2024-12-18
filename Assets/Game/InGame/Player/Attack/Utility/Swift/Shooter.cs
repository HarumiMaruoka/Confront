using Confront.Player;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class Shooter
    {
        public Projectile ProjectilePrefab;
        public float FireTime;

        private bool _fired = false;

        public void Reset()
        {
            _fired = false;
        }

        public void Update(PlayerController player, float elapsed)
        {
            if (FireTime <= elapsed && !_fired)
            {
                Fire(player);
                _fired = true;
            }
        }

        public void Fire(PlayerController player)
        {
            if(ProjectilePrefab == null)
            {
                Debug.LogError("ProjectilePrefab is null.");
                return;
            }
            var projectile = GameObject.Instantiate(ProjectilePrefab, player.transform.position, Quaternion.identity);
            projectile.Initialize(player);
        }
    }
}