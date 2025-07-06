using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Equipment
{
    public class EquipmentInventory : IEnumerable<EquipmentItem>
    {
        private List<EquipmentItem> _slots = new List<EquipmentItem>();

        public event Action<int, EquipmentItem> OnItemAdded;
        public event Action<int, EquipmentItem> OnItemRemoved;
        public event Action<int, EquipmentItem> OnItemInserted;
        public event Action<int> OnInventoryResized;

        public EquipmentInventory(int capacity = 20)
        {
            _slots = new List<EquipmentItem>(capacity);
        }

        public bool AddItem(EquipmentItem item)
        {
            Debug.Assert(item != null, "Cannot add null item to inventory.");
            Debug.Assert(_slots.Contains(item), item + " is already in the inventory."); // 同じインスタンスは追加しない。

            // 空のスロットを探して追加する。
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] == null)
                {
                    _slots[i] = item;
                    OnItemAdded?.Invoke(i, item);
                    return true;
                }
            }

            // 空のスロットがない場合は、falseを返す。
            return false;
        }

        public bool RemoveItem(EquipmentItem item)
        {
            int index = _slots.IndexOf(item);
            if (index != -1)
            {
                _slots.RemoveAt(index);
                OnItemRemoved?.Invoke(index, item);
                return true;
            }
            else
            {
                // アイテムが見つからなかった場合のデバッグアサーション
                Debug.LogWarning($"Attempted to remove item that is not in the inventory: {item}");
                return false;
            }
        }

        public EquipmentItem Insert(EquipmentItem item, int index)
        {
            Debug.Assert(index >= 0 && index < _slots.Count, "Index out of bounds for inventory.");

            // 古いアイテムを取得して、スロットに新しいアイテムを挿入する。
            var prev = _slots[index];
            _slots[index] = item;

            OnItemInserted?.Invoke(index, item);
            return prev;
        }

        public List<EquipmentItem> Resize(int newSize)
        {
            if (newSize < 0)
            {
                Debug.LogError("New size must be non-negative.");
                return null;
            }

            if (newSize == _slots.Count)
            {
                // サイズが変わらない場合は何もしない。
                return null;
            }

            // 新しいサイズが現在のサイズより小さい場合、余分なアイテムを削除する。
            // 削除されたアイテムをリストに追加して返す。
            if (newSize < _slots.Count)
            {
                var removedItems = new List<EquipmentItem>();
                for (int i = newSize; i < _slots.Count; i++)
                {
                    removedItems.Add(_slots[i]);
                }
                _slots.RemoveRange(newSize, _slots.Count - newSize);
                OnInventoryResized?.Invoke(newSize);
                return removedItems;
            }

            // 新しいサイズが現在のサイズより大きい場合、nullで埋める。
            while (_slots.Count < newSize)
            {
                _slots.Add(null);
            }
            OnInventoryResized?.Invoke(newSize);
            return null;
        }

        public int Count => _slots.Count;
        public EquipmentItem this[int index] { get => _slots[index]; set => _slots[index] = value; }
        public IEnumerator<EquipmentItem> GetEnumerator() => _slots.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}