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

        //public WeaponInstance(WeaponData data)
        //{
        //    Data = data;
        //}

        //public WeaponInstance(int id)
        //{
        //    Data = WeaponManager.WeaponSheet.GetWeaponData(id);
        //}

        public static WeaponInstance Create(int id)
        {
            return new WeaponInstance { ID = id };
        }

        //public WeaponInstance(string name)
        //{
        //    Data = WeaponManager.WeaponSheet.GetWeaponData(name);
        //}

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