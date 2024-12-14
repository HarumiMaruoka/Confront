using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.ActionItem
{
    [CreateAssetMenu(fileName = "ActionItemSheet", menuName = "Game Data Sheets/ActionItemSheet")]
    public class ActionItemSheet : NexEditor.GameDataSheet.SheetBase<ActionItemData>
    {
        private Dictionary<int, ActionItemData> _idToItemDic = new Dictionary<int, ActionItemData>();

        public void Initialize()
        {
            foreach (var item in this)
            {
                _idToItemDic.Add(item.ID, item);
            }
        }

        public ActionItemData GetItem(int id)
        {
            return _idToItemDic[id];
        }

        public bool Contains(int id)
        {
            return _idToItemDic.ContainsKey(id);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ActionItemSheet))]
    public class ItemSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                ActionItemSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}