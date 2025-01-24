using System;
using UnityEngine;

namespace Confront.Weapon
{
    public static class WeaponInstanceExtensions
    {
        public static string ToAnimationPrefix(this WeaponInstance weaponInstance)
        {
            return weaponInstance.Data.AnimationPrefix;
        }
    }
}