﻿using Confront.Player;
using System;
using UnityEngine;

namespace Confront.ActionItem
{
    public abstract class Potion : ItemEffect
    {
        public abstract void ApplyEffect(PlayerController player, InventorySlot slot);
    }
}