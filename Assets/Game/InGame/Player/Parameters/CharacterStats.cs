using Confront.Debugger;
using System;
using UnityEngine;

namespace Confront.Player
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "ConfrontSO/Player/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {

        public float MaxHealth = 100;
        [SerializeField] private float _attackPower = 1;
        public float AttackPower => DebugParams.Instance.IsInfiniteAttackPower ? 9999 : _attackPower;
        public float Defense = 1;
        public float Weight = 1;
    }
}