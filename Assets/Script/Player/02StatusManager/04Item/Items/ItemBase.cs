using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class ItemBase
    {
        public virtual void Use()
        {
            Debug.Log("未実装です。");
        }

        private string _name = "未設定";
        public string Name => _name;

        private string _explanatoryText = "未設定";
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
        /// <summary> 未設定 </summary>
        NotSet,
        /// <summary> 回復 </summary>
        Heal,
        /// <summary> パワーアップ </summary>
        PowerUp,
        /// <summary> 弾（矢とか銃の弾とか） </summary>
        Bullet,
        /// <summary> カギ </summary>
        Key
    }
}