using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "VampireBatStats", menuName = "ConfrontSO/Enemy/VampireBat/Stats")]
    public class VampireBatStats : ScriptableObject
    {
        public float Health;
        public float Defense;
        public float AttackPower;
    }
}