using Confront.Player.Combo;
using System;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponData : ScriptableObject
    {
        public int ID;
        public string AnimationPrefix;
        public string Name;
        public string Description;
        public Sprite Icon;
        public ComboTree ComboTree;
    }
}