using System;
using UnityEngine;

namespace Confront.Item
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField]
        private ItemData _item;
        [SerializeField]
        private int _count;

        public ItemData Item // スロットに格納されているアイテム
        {
            get => _item; set
            {
                _item = value;
                OnAnyChanged?.Invoke(this);
            }
        }
        public int Count // アイテムの数
        {
            get => _count; set
            {
                if (!_item)
                {
                    _count = 0;
                    return;
                }
                if (value < 0 || value > _item.MaxStack)
                {
                    Debug.LogError("Count must be greater than 0 and less than or equal to MaxStack.");
                    return;
                }

                _count = value;
                if (_count == 0)
                {
                    _item = null;
                }
                OnAnyChanged?.Invoke(this);
            }
        }

        public event Action<InventorySlot> OnAnyChanged;

        /// <summary> アイテムを追加する </summary>
        /// <param name="newItem"> 追加するアイテム </param>
        /// <param name="amount"> 追加する数 </param>
        /// <returns> 追加できなかった数 </returns>
        public int AddItem(ItemData newItem, int amount)
        {
            if (Item == null || Item.ID == newItem.ID)
            {
                int availableSpace = (Item == null ? newItem.MaxStack : newItem.MaxStack - Count); // スタック可能な残りのスペース
                int addAmount = Mathf.Min(amount, availableSpace);

                Item = newItem;
                Count += addAmount;
                OnAnyChanged?.Invoke(this);
                return amount - addAmount;
            }
            return amount;
        }

        /// <summary> アイテムを削除する </summary>
        /// <param name="item"> 削除するアイテム </param>
        /// <param name="amount"> 削除する数 </param>
        /// <returns> 削除できなかった数 </returns>
        public int RemoveItem(ItemData item, int amount)
        {
            if (item == null || Item.ID != item.ID)
            {
                return amount;
            }
            int removeAmount = Mathf.Min(amount, Count);
            Count -= removeAmount;
            if (Count == 0)
            {
                this.Item = null;
            }
            OnAnyChanged?.Invoke(this);
            return amount - removeAmount;
        }
    }
}