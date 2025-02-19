using System;
using UnityEngine;

namespace Confront.Weapon
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "ConfrontSO/Weapon/Stats")]
    public class Stats : ScriptableObject
    {
        public float AttackPower = 1f;
    }
}