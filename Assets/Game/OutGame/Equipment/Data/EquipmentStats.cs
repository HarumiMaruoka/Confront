using UnityEngine;

namespace Confront.Equipment
{
    public struct EquipmentStats
    {
        /// <summary> 増加体力 </summary>
        public int Health;
        /// <summary> 増加スタミナ </summary>
        public int Stamina;
        /// <summary> 増加攻撃力 </summary>
        public int Damage;
        /// <summary> 増加防御力 </summary>
        public int Armor;
        /// <summary> 増加移動速度 </summary>
        public int Speed;
        /// <summary> 増加クリティカル率 </summary>
        public int CritChance;
        /// <summary> 増加クリティカルダメージ </summary>
        public int CritDamage;
        /// <summary> 減少回避インターバル </summary>
        public int DodgeInterval;

        public EquipmentStats(int health, int stamina, int damage, int armor, int speed, int critChance, int critDamage, int dodgeInterval)
        {
            Health = health;
            Stamina = stamina;
            Damage = damage;
            Armor = armor;
            Speed = speed;
            CritChance = critChance;
            CritDamage = critDamage;
            DodgeInterval = dodgeInterval;
        }

        public static EquipmentStats operator +(EquipmentStats a, EquipmentStats b)
        {
            return new EquipmentStats(
                a.Health + b.Health,
                a.Stamina + b.Stamina,
                a.Damage + b.Damage,
                a.Armor + b.Armor,
                a.Speed + b.Speed,
                a.CritChance + b.CritChance,
                a.CritDamage + b.CritDamage,
                a.DodgeInterval + b.DodgeInterval
            );
        }
    }
}