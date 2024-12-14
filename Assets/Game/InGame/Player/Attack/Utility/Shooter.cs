using Confront.Player;
using Cysharp.Threading.Tasks.Triggers;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    [Serializable]
    public class Shooter
    {
        public Projectile ProjectilePrefab;
        public float FireTime;
        public Vector3 SpawnOffset;

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
            var position = player.transform.position + player.transform.rotation * SpawnOffset;
            var projectile = GameObject.Instantiate(ProjectilePrefab, position, player.transform.rotation);
            projectile.Initialize(player, chargeAmount);
        }
    }
}