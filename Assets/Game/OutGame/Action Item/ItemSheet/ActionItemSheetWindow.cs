#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.ActionItem
{
    public class ActionItemSheetWindow : NexEditor.ScriptableDashboard.ScriptableDashboardEditor<ActionItemData>
    {
        [MenuItem("Window/Game Data Sheet/ActionItemSheetWindow")]
        public static void Init()
        {
            ActionItemSheetWindow window = GetWindow<ActionItemSheetWindow>();
            string[] guids = AssetDatabase.FindAssets("t:ActionItemSheet");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var dashboard = AssetDatabase.LoadAssetAtPath<ActionItemSheet>(path);
                if (dashboard != null)
                {
                    window.Setup(dashboard);
                }
                else
                {
                    Debug.LogWarning("ActionItemSheet not found at path: " + path);
                }
            }
            else
            {
                Debug.LogWarning("No ActionItemSheet found in the project.");
            }
        }

        public static void ShowWindow(ActionItemSheet dashboard)
        {
            ActionItemSheetWindow window = GetWindow<ActionItemSheetWindow>();
            window.Setup(dashboard);
        }
    }
}
#endif