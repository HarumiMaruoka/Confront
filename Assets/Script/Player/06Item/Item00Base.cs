using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class Item00Base
    {
        public virtual void Use()
        {
            Debug.Log("–¢ŽÀ‘•‚Å‚·B");
        }

        private string _name = "–¢Ý’è";
        public string Name => _name;

        private string _explanatoryText = "–¢Ý’è";
        public string ExplanatoryText => _explanatoryText;

        private ItemType _type = ItemType.NotSet;
        public ItemType Type => _type;

        public Item00Base(string name, 
            string explanatoryText,
            ItemType type)
        {
            _name = name;
            _explanatoryText = explanatoryText;
            _type = type;
        }
    }
    public enum ItemType
    {
        NotSet,
        Heal,
        PowerUp,
        Bullet,
        Key
    }
}