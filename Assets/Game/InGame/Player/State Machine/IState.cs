using System;
using UnityEngine;

namespace Confront.Player
{
    public interface IState
    {
        string AnimationName { get; }
        void Enter(PlayerController player) { }
        void Execute(PlayerController player) { }
        void LateExecute(PlayerController player) { }
        void Exit(PlayerController player) { }
    }
}