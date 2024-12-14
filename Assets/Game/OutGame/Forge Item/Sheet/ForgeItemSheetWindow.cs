#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.ForgeItem
{
    public class ForgeItemSheetWindow : NexEditor.GameDataSheet.SheetWindowBase<ForgeItemData, ForgeItemSheet, ForgeItemSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/ForgeItemSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(ForgeItemSheetWindow)).Show();
        }
    }
}
#endif