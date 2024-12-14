using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.ForgeItem
{
    public class ForgeItemInventory : IEnumerable<KeyValuePair<ForgeItemData, int>>
    {
        [SerializeField]
        private int _capacity = 100;
        [SerializeField]
        private Dictionary<ForgeItemData, int> _itemCountMap = new Dictionary<ForgeItemData, int>();

        public event Action<ForgeItemData, int> OnCountChanged;

        public int Pickup(int id, int amount)
        {
            var data = ForgeItemManager.ForgeItemSheet.GetItem(id);
            return Pickup(data, amount);
        }

        /// <returns> The amount that could not be picked up. </returns>
        public int Pickup(ForgeItemData data, int amount)
        {
            if (_itemCountMap.ContainsKey(data))
            {
                if (_itemCountMap[data] < _capacity)
                {
                    int remainAmount = Mathf.Min(_capacity - _itemCountMap[data], amount);
                    _itemCountMap[data] += remainAmount;
                    OnCountChanged?.Invoke(data, _itemCountMap[data]);
                    return amount - remainAmount;
                }
            }
            else
            {
                if (_itemCountMap.Count < _capacity)
                {
                    _itemCountMap.Add(data, amount);
                    OnCountChanged?.Invoke(data, amount);
                    return 0;
                }
            }
            return amount;
        }

        public bool Use(ForgeItemData data, int amount)
        {
            if (_itemCountMap.ContainsKey(data))
            {
                if (_itemCountMap[data] >= amount)
                {
                    _itemCountMap[data] -= amount;
                    OnCountChanged(data, _itemCountMap[data]);
                    return true;
                }
            }
            return false;
        }

        public int GetCount(ForgeItemData data)
        {
            if (_itemCountMap.ContainsKey(data))
            {
                return _itemCountMap[data];
            }
            return 0;
        }

        public bool HasEnoughItems(ForgeItemData data, int amount)
        {
            return GetCount(data) >= amount;
        }

        public void Clear()
        {
            _itemCountMap.Clear();
        }

        #region IEnumerable
        public int Count => _itemCountMap.Count;

        public ForgeItemData this[int index]
        {
            get
            {
                int i = 0;
                foreach (var pair in _itemCountMap)
                {
                    if (i == index)
                    {
                        return pair.Key;
                    }
                    i++;
                }
                return null;
            }
        }

        public IEnumerator<KeyValuePair<ForgeItemData, int>> GetEnumerator()
        {
            return _itemCountMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}