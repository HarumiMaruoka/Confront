#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.ForgeItem
{
    public class ForgeItemSheetWindow : NexEditor.ScriptableDashboard.ScriptableDashboardEditor<ForgeItemData>
    {
        [MenuItem("Window/Game Data Sheet/ForgeItemSheetWindow")]
        public static void Init()
        {
            ForgeItemSheetWindow window = GetWindow<ForgeItemSheetWindow>();
            string[] guids = AssetDatabase.FindAssets("t:ForgeItemSheet");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var dashboard = AssetDatabase.LoadAssetAtPath<ForgeItemSheet>(path);
                if (dashboard != null)
                {
                    window.Setup(dashboard);
                }
                else
                {
                    Debug.LogWarning("ForgeItemSheet not found at path: " + path);
                }
            }
            else
            {
                Debug.LogWarning("No ForgeItemSheet found in the project.");
            }
        }

        public static void ShowWindow(ForgeItemSheet dashboard)
        {
            ForgeItemSheetWindow window = GetWindow<ForgeItemSheetWindow>();
            window.Setup(dashboard);
        }
    }
}
#endif