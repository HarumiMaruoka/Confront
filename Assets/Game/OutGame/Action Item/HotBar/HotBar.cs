using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.ActionItem
{
    public class HotBar
    {
        public HotBar()
        {
            _hotBarItems = new Dictionary<Direction, InventorySlot>()
            {
                { Direction.Top, new InventorySlot() },
                { Direction.Bottom, new InventorySlot() },
                { Direction.Right, new InventorySlot() },
                { Direction.Left, new InventorySlot() },
            };
        }

        [SerializeField]
        private Dictionary<Direction, InventorySlot> _hotBarItems;

        public InventorySlot GetSlot(Direction direction)
        {
            return _hotBarItems[direction];
        }
    }
}