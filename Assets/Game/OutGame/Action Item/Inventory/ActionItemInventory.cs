using Confront.NotificationManager;
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

            ShowNotification(item, amount - remainAmount, remainAmount);
            return remainAmount;
        }

        private void ShowNotification(ActionItemData item, int addAmount, int remainAmount)
        {
            var title = $"ActionItem";
            var message = $"{item.Name}:addAmount:{addAmount}";
            if (remainAmount > 0) message += $",remainAmount:{remainAmount}";
            var icon = item.Icon;
            Notifier.AddNotification(title, message, icon);
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