using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponInventory : IEnumerable<WeaponInstance>
    {
        [SerializeField]
        private List<WeaponInstance> _weapons;
        [SerializeField]
        private int _maxSize;

        public WeaponInventory(int maxSize = 20)
        {
            _weapons = new List<WeaponInstance>();
            _maxSize = maxSize;
        }

        public int Count => _weapons.Count;

        public event Action<WeaponInstance> OnWeaponAdded;
        public event Action<WeaponInstance, WeaponInstance> OnWeaponSwapped;
        public event Action<WeaponInstance> OnWeaponRemoved;

        public WeaponInstance this[int index] => _weapons[index];

        public bool AddWeapon(int id)
        {
            var weapon = new WeaponInstance(id);
            return AddWeapon(weapon);
        }

        public bool AddWeapon(int id, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var weapon = new WeaponInstance(id);
                if (!AddWeapon(weapon)) return false;
            }
            return true;
        }

        public bool AddWeapon(WeaponInstance weapon)
        {
            if (_weapons.Count >= _maxSize)
            {
                Debug.LogWarning("Inventory is full");
                return false;
            }
            _weapons.Add(weapon);
            OnWeaponAdded?.Invoke(weapon);
            return true;
        }

        public void RemoveWeapon(WeaponInstance weapon)
        {
            _weapons.Remove(weapon);
            OnWeaponRemoved?.Invoke(weapon);
        }

        public void RemoveWeapon(int index)
        {
            var weapon = _weapons[index];
            _weapons.RemoveAt(index);
            OnWeaponRemoved?.Invoke(weapon);
        }

        public void SwapWeapons(int index1, int index2)
        {
            var temp = _weapons[index1];
            _weapons[index1] = _weapons[index2];
            _weapons[index2] = temp;

            OnWeaponSwapped?.Invoke(_weapons[index1], _weapons[index2]);
        }

        public void Resize(int newSize)
        {
            if (newSize < _weapons.Count)
            {
                // サイズを小さくすることはできません。
                Debug.LogWarning("New size is smaller than current size");
                return;
            }
            _maxSize = newSize;
        }

        public IEnumerator<WeaponInstance> GetEnumerator()
        {
            return _weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}