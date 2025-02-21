using System;
using UnityEngine;

namespace Confront.Stage.Utility
{
    public class PushBackZone : MonoBehaviour
    {
        [SerializeField] private float _height = 0.0f;

        private void Update()
        {
            var player = Player.PlayerController.Instance;
            if (!player) return;
            if (player.transform.position.y < _height)
            {
                var prev = player.CharacterController.enabled;
                player.CharacterController.enabled = false;
                player.transform.position = new Vector3(player.transform.position.x, _height, player.transform.position.z);
                player.CharacterController.enabled = prev;
            }
        }
    }
}