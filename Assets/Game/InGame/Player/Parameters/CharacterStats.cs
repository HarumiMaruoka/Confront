using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "ConfrontSO/Player/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public float MaxHealth = 100;
        public float AttackPower = 1;
        public float Defense = 1;
        public float Weight = 1;
    }
}