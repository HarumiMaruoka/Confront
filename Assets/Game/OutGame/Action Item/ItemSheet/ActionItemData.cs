using System;
using UnityEngine;

namespace Confront.ActionItem
{
    public class ActionItemData : ScriptableObject
    {
        public int ID;
        public string Name;
        public string Description;
        public int MaxStack;
        public Sprite Icon;
        public ItemEffect ItemEffect;
    }
}