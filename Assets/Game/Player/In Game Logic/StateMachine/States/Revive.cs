using System;
using UnityEngine;

namespace Confront.Player.States
{
    public class Revive : IPlayerState
    {
        public string AnimationStateName => "Revive";

        public bool IsMovementInputEnabled => false;

        public void Enter(PlayerController player)
        {

        }

        public void Update(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {

        }
    }
}