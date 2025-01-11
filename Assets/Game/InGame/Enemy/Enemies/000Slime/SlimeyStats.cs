using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    [CreateAssetMenu(fileName = "SlimeyStats", menuName = "ConfrontSO/Enemy/Slimey/Stats")]
    public class SlimeyStats : ScriptableObject
    {
        public float Health = 100f;
        public float AttackPower = 10f;
        public float Defense = 0f;
    }
}