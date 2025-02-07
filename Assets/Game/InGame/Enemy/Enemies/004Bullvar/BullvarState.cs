using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    public abstract class BullvarState : ScriptableObject
    {
        public abstract string AnimationName { get; }
        public virtual void Enter(PlayerController player, BullvarController controller) { }
        public virtual void Execute(PlayerController player, BullvarController controller) { }
        public virtual void Exit(PlayerController player, BullvarController controller) { }
    }
}