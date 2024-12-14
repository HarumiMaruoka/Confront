using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.ForgeItem
{
    [CreateAssetMenu(fileName = "ForgeItemSheet", menuName = "Game Data Sheets/ForgeItemSheet")]
    public class ForgeItemSheet : NexEditor.GameDataSheet.SheetBase<ForgeItemData>
    {
        public void Initialize()
        {
            foreach (var data in this)
            {
                _idToData.Add(data.ID, data);
                _nameToData.Add(data.Name, data);
            }
        }

        private Dictionary<int, ForgeItemData> _idToData = new Dictionary<int, ForgeItemData>();
        private Dictionary<string, ForgeItemData> _nameToData = new Dictionary<string, ForgeItemData>();

        public ForgeItemData GetItem(int id)
        {
            if (_idToData.TryGetValue(id, out var data))
            {
                return data;
            }
            Debug.LogError($"ForgeItemData not found. ID: {id}");
            return null;
        }

        public ForgeItemData GetItem(string name)
        {
            if (_nameToData.TryGetValue(name, out var data))
            {
                return data;
            }
            Debug.LogError($"ForgeItemData not found. Name: {name}");
            return null;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ForgeItemSheet))]
    public class ForgeItemSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                ForgeItemSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}