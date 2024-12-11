using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Item
{
    [CreateAssetMenu(fileName = "ItemSheet", menuName = "Game Data Sheets/ItemSheet")]
    public class ItemSheet : NexEditor.GameDataSheet.SheetBase<ItemData>
    {
        private Dictionary<int, ItemData> _idToItemDic = new Dictionary<int, ItemData>();

        public void Initialize()
        {
            foreach (var item in this)
            {
                _idToItemDic.Add(item.ID, item);
            }
        }

        public ItemData GetItem(int id)
        {
            return _idToItemDic[id];
        }

        public bool Contains(int id)
        {
            return _idToItemDic.ContainsKey(id);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ItemSheet))]
    public class ItemSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                ItemSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}