using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Item
{
    public abstract class ItemEffect : ScriptableObject
    {
        public abstract void ApplyEffect(PlayerController player);
    }
}