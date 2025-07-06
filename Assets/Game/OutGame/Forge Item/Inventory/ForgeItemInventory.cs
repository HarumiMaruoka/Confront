using Confront.NotificationManager;
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

        public bool Pickup(int id, int amount)
        {
            var data = ForgeItemManager.ForgeItemSheet.GetItem(id);
            return Pickup(data, amount);
        }

        public bool Pickup(ForgeItemData data, int amount)
        {
            if (data == null)
            {
                Debug.LogError("ForgeItemData is null.");
                return false;
            }

            if (!_itemCountMap.ContainsKey(data))
            {
                //Debug.Log($"Adding new item to inventory: {data.name}");
                _itemCountMap[data] = 0;
            }

            if (amount <= 0)
            {
                Debug.LogWarning("Amount to pickup must be greater than 0.");
                return false;
            }

            if (_itemCountMap[data] >= _capacity) // すでに最大数に達している場合は、追加できない。
            {
                Debug.LogWarning($"Cannot pickup {data.name}. Inventory is full.");
                return false;
            }

            var prev = _itemCountMap[data];
            _itemCountMap[data] += amount;
            _itemCountMap[data] = Mathf.Clamp(_itemCountMap[data], 0, _capacity);

            ShowNotification(data, amount);

            return true;
        }

        public bool Remove(ForgeItemData data, int amount)
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

        public bool Remove(int id, int amount)
        {
            var data = ForgeItemManager.ForgeItemSheet.GetItem(id);
            return Remove(data, amount);
        }

        public int GetCount(ForgeItemData data)
        {
            if (_itemCountMap.ContainsKey(data))
            {
                return _itemCountMap[data];
            }

            Debug.LogError($"ForgeItemData not found in inventory: {data?.name}");
            return 0;
        }

        public int GetCount(int id)
        {
            var data = ForgeItemManager.ForgeItemSheet.GetItem(id);
            return GetCount(data);
        }

        // アイテムが十分にあるかを確認する。
        public bool HasEnoughItems(ForgeItemData data, int amount)
        {
            return GetCount(data) >= amount;
        }

        public bool HasEnoughItems(int id, int amount)
        {
            var data = ForgeItemManager.ForgeItemSheet.GetItem(id);
            return HasEnoughItems(data, amount);
        }

        public void Clear()
        {
            _itemCountMap.Clear();
        }

        private void ShowNotification(ForgeItemData item, int addAmount)
        {
            var title = $"Picked up {item.Name} +{addAmount}";
            var message = ""; // $"{item.Name}:addAmount:{addAmount}";
            var icon = item.Icon;
            Notifier.AddNotification(title, message, icon);
        }

        #region IEnumerable
        public int Count => _itemCountMap.Count;

        public ForgeItemData this[int index]
        {
            get
            {
                if (index < 0 || index >= _itemCountMap.Count)
                {
                    Debug.LogError($"Index {index} is out of range for ForgeItemInventory.");
                    return null;
                }

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