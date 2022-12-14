using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class ItemBase
    {
        public virtual void Use()
        {
            Debug.Log("???????ł??B");
        }

        private string _name = "???ݒ?";
        public string Name => _name;

        private string _explanatoryText = "???ݒ?";
        public string ExplanatoryText => _explanatoryText;

        private ItemType _type = ItemType.NotSet;
        public ItemType Type => _type;

        public ItemBase(string name, 
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
        /// <summary> ???ݒ? </summary>
        NotSet,
        /// <summary> ???? </summary>
        Heal,
        /// <summary> ?p???[?A?b?v </summary>
        PowerUp,
        /// <summary> ?e?i???Ƃ??e?̒e?Ƃ??j </summary>
        Bullet,
        /// <summary> ?J?M </summary>
        Key
    }
}