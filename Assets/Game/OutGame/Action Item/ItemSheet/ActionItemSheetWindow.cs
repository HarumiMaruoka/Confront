#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.ActionItem
{
    public class ActionItemSheetWindow : NexEditor.GameDataSheet.SheetWindowBase<ActionItemData, ActionItemSheet, ActionItemSheetWindowLayout>
    {
        [MenuItem("Window/Game Data Sheet/ActionItemSheetWindow")]
        public static void Init()
        {
            GetWindow(typeof(ActionItemSheetWindow)).Show();
        }
    }
}
#endif