using System;
using UnityEngine;

namespace Confront.Player.States
{
    public class MidAir : IPlayerState
    {
        public string AnimationStateName => "MidAir";

        public bool IsMovementInputEnabled => true;

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