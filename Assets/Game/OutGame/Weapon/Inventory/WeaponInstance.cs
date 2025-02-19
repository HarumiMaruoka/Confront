using System;
using UnityEngine;

namespace Confront.Weapon
{
    [Serializable]
    public class WeaponInstance
    {
        public int ID;
        public WeaponData Data => WeaponManager.WeaponSheet.GetWeaponData(ID);
        public int Level = 1;

        public static WeaponInstance Create(int id)
        {
            return new WeaponInstance { ID = id };
        }

        public float AttackPower
        {
            get
            {
                if (Data.Stats) return Data.Stats.AttackPower;
                return 1;
            }
        }

        public void Equip()
        {
            Debug.Log($"Equipped {Data.Name}");
        }

        public void Unequip()
        {
            Debug.Log($"Unequipped {Data.Name}");
        }
    }
}