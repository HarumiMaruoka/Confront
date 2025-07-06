#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Confront.Enemy
{
    public class EnemySheetWindow : NexEditor.ScriptableDashboard.ScriptableDashboardEditor<EnemyData>
    {
        [MenuItem("Window/Game Data Sheet/EnemySheetWindow")]
		public static void Init()
		{
			EnemySheetWindow window = GetWindow<EnemySheetWindow>();
			string[] guids = AssetDatabase.FindAssets("t:EnemySheet");
			if (guids.Length > 0)
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[0]);
				var dashboard = AssetDatabase.LoadAssetAtPath<EnemySheet>(path);
				if (dashboard != null)
				{
					window.Setup(dashboard);
				}
				else
				{
					Debug.LogWarning("EnemySheet not found at path: " + path);
				}
			}
			else
			{
				Debug.LogWarning("No EnemySheet found in the project.");
			}
		}

		public static void ShowWindow(EnemySheet dashboard)
		{
			EnemySheetWindow window = GetWindow<EnemySheetWindow>();
			window.Setup(dashboard);
		}
	}
}
#endif