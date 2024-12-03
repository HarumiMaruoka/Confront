using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Confront/Player/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public float MaxHealth = 100;
        public float AttackPower = 1;
    }
}