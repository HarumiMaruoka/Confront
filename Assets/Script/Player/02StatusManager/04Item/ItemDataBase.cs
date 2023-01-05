using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 全てのアイテムのデータを管理し提供するクラス
    /// </summary>
    [System.Serializable]
    public class ItemDataBase
    {
        private ItemBase[] _itemData = new ItemBase[Constants.MaxItemID];
        /// <summary>
        /// 全てのアイテムのデータ <br/>
        /// インデックスにIDを指定することで、指定のアイテムデータにアクセスできる。
        /// </summary>
        public ItemBase[] ItemData => _itemData;

        /// <summary>
        /// Addressable Assets Systemを利用してデータを読み込む。
        /// </summary>
        public void LoadItem(string addressableName)
        {

        }
    }
}