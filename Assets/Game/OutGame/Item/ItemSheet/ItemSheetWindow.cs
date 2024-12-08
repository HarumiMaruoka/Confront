#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.Item
{
    public class ItemSheetWindow : NexEditor.GameDataSheet.SheetWindowBase<ItemData, ItemSheet, ItemSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/ItemSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(ItemSheetWindow)).Show();
        }
    }
}
#endif