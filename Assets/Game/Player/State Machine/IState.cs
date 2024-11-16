using System;
using UnityEngine;

namespace Confront.Player
{
    public interface IState
    {
        void Enter(PlayerController player);
        void Update(PlayerController player);
        void Exit(PlayerController player);
    }
}