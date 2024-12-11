using System;
using UnityEngine;

namespace Confront.Item
{
    public class ItemInventory
    {
        [SerializeField]
        private InventorySlot[] _slots = new InventorySlot[MaxSlotCount];
        private const int MaxSlotCount = 20;

        public int Count => _slots.Length;

        public InventorySlot this[int index] { get => _slots[index]; set => _slots[index] = value; }

        public ItemInventory()
        {
            for (int i = 0; i < MaxSlotCount; i++)
            {
                _slots[i] = new InventorySlot();
            }
        }

        public int AddItem(ItemData item, int amount)
        {
            int remainAmount = amount;
            foreach (var slot in _slots)
            {
                remainAmount = slot.AddItem(item, remainAmount);
                if (remainAmount == 0)
                {
                    break;
                }
            }
            return remainAmount;
        }

        public void RemoveItem(ItemData item, int amount)
        {
            int remainAmount = amount;
            foreach (var slot in _slots)
            {
                remainAmount = slot.RemoveItem(item, remainAmount);
                if (remainAmount == 0)
                {
                    break;
                }
            }
        }
    }
}