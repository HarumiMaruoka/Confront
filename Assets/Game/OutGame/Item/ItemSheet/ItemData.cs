using System;
using UnityEngine;

namespace Confront.Item
{
    public class ItemData : ScriptableObject
    {
        public int ID;
        public string Name;
        public string Description;
        public int MaxStack;
        public Sprite Icon;
        public ItemEffect ItemEffect;
    }
}