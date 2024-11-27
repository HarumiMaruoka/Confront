using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public abstract class AttackBehaviour : ScriptableObject, IState
    {
        public ComboInput ComboInput = ComboInput.None;

        public abstract string AnimationName { get; }

        public abstract void Enter(PlayerController player);

        public abstract void Execute(PlayerController player);

        public abstract void Exit(PlayerController player);
    }

    public enum ComboInput
    {
        None,
        X,
        Y
    }
}