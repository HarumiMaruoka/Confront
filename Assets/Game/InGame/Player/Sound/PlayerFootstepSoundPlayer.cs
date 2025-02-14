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
            _playerFootstepSound.PlayFootstepSound();
        }
    }
}