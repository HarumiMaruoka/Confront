using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    public abstract class VampireBatState : ScriptableObject
    {
        public abstract string AnimationName { get; }

        public abstract void Enter(PlayerController player, VampireBatController vampireBat);
        public abstract void Execute(PlayerController player, VampireBatController vampireBat); // UpdateはScriptableObjectによって予約されているため、Executeに変更。
        public abstract void Exit(PlayerController player, VampireBatController vampireBat);
    }
}