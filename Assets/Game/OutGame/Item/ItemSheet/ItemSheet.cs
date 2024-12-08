using System;
using UnityEngine;

namespace Confront.Item
{
    [CreateAssetMenu(fileName = "ItemSheet",menuName = "Game Data Sheets/ItemSheet")]
    public class ItemSheet : NexEditor.GameDataSheet.SheetBase<ItemData>
    {

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