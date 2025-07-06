#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponSheetWindow : NexEditor.ScriptableDashboard.ScriptableDashboardEditor<WeaponData>
    {
        [MenuItem("Window/Game Data Sheet/WeaponSheetWindow")]
        public static void Init()
        {
            WeaponSheetWindow window = GetWindow<WeaponSheetWindow>();
            string[] guids = AssetDatabase.FindAssets("t:WeaponSheet");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                var dashboard = AssetDatabase.LoadAssetAtPath<WeaponSheet>(path);
                if (dashboard != null)
                {
                    window.Setup(dashboard);
                }
                else
                {
                    Debug.LogWarning("WeaponSheet not found at path: " + path);
                }
            }
            else
            {
                Debug.LogWarning("No WeaponSheet found in the project.");
            }
        }

        public static void ShowWindow(WeaponSheet dashboard)
        {
            WeaponSheetWindow window = GetWindow<WeaponSheetWindow>();
            window.Setup(dashboard);
        }
    }
}
#endif