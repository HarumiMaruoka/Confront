using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class PlayerFootstepSoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private PlayerFootstepSound _playerFootstepSound;

        private void OnTriggerEnter(Collider other)
        {
            var groundLayer = LayerUtility.GroundLayerMask | LayerUtility.EnemyLayerMask | LayerUtility.PlatformEnemy;
            var foot = 1 << other.gameObject.layer;
            var isOnGroundLayer = (groundLayer & foot) != 0;

            if (isOnGroundLayer) _playerFootstepSound.PlayFootstepSound();
        }
    }
}