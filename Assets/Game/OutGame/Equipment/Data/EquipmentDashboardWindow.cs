#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using NexEditor.ScriptableDashboard;

namespace Confront.Equipment
{
    public class EquipmentDashboardWindow : ScriptableDashboardEditor<EquipmentData>
    {
        [MenuItem("Window/Scriptable Dashboard/EquipmentDashboardWindow")]
        public static void ShowWindow()
        {
            EquipmentDashboardWindow window = GetWindow<EquipmentDashboardWindow>();
            string[] guids = AssetDatabase.FindAssets("t:EquipmentDashboard");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var dashboard = AssetDatabase.LoadAssetAtPath<EquipmentDashboard>(path);
                if (dashboard != null)
                {
                    window.Setup(dashboard);
                }
                else
                {
                    Debug.LogWarning("EquipmentDashboard not found at path: " + path);
                }
            }
            else
            {
                Debug.LogWarning("No EquipmentDashboard found in the project.");
            }
        }
        public static void ShowWindow(EquipmentDashboard dashboard)
        {
            EquipmentDashboardWindow window = GetWindow<EquipmentDashboardWindow>();
            window.Setup(dashboard);
        }
    }

    [UnityEditor.CustomEditor(typeof(EquipmentDashboard))]
    public class EquipmentDashboardDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open Window"))
            {
                var dashboard = target as EquipmentDashboard;
                if (dashboard != null)
                {
                    EquipmentDashboardWindow.ShowWindow(dashboard);
                }
            }
        }

        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (UnityEditor.Selection.activeObject is EquipmentDashboard dashboard)
            {
                EquipmentDashboardWindow.ShowWindow(dashboard);
                return true;
            }
            return false;
        }
    }
}
#endif