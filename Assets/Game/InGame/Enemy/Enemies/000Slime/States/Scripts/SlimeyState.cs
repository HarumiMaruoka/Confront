using System;
using Confront.Player;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    public abstract class SlimeyState : ScriptableObject
    {
        public abstract string AnimationName { get; }
        public abstract void Enter(PlayerController player, SlimeyController slimey);
        public abstract void Execute(PlayerController player, SlimeyController slimey); // UpdateはScriptableObjectによって予約されているため、Executeに変更。
        public abstract void Exit(PlayerController player, SlimeyController slimey);
    }
}