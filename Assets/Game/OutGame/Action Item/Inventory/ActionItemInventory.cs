using System;
using UnityEngine;

namespace Confront.ActionItem
{
    public class ActionItemInventory
    {
        [SerializeField]
        private InventorySlot[] _slots = new InventorySlot[MaxSlotCount];
        private const int MaxSlotCount = 20;

        public int Count => _slots.Length;

        public InventorySlot this[int index] { get => _slots[index]; set => _slots[index] = value; }

        public ActionItemInventory()
        {
            for (int i = 0; i < MaxSlotCount; i++)
            {
                _slots[i] = new InventorySlot();
            }
        }

        public int AddItem(int id, int amount)
        {
            var item = ActionItemManager.ActionItemSheet.GetItem(id);
            return AddItem(item, amount);
        }

        public int AddItem(ActionItemData item, int amount)
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

        public void RemoveItem(ActionItemData item, int amount)
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