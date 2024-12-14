using Confront.Player;
using System;
using UnityEngine;

namespace Confront.ActionItem
{
    public abstract class ItemEffect : ScriptableObject
    {
        public abstract void UseItem(PlayerController player, InventorySlot slot);
    }
}