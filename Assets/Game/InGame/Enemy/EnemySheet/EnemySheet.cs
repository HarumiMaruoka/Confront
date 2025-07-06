using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy
{
	[CreateAssetMenu(fileName = "EnemySheet", menuName = "Game Data Sheets/EnemySheet")]
	public class EnemySheet : NexEditor.ScriptableDashboard.ScriptableDashboard<EnemyData>
	{
		public void Initialize()
		{
			foreach (var data in this)
			{
				_idToData[data.ID] = data;
				_nameToData[data.Name] = data;
			}
		}

		private Dictionary<int, EnemyData> _idToData = new Dictionary<int, EnemyData>();
		private Dictionary<string, EnemyData> _nameToData = new Dictionary<string, EnemyData>();

		public IReadOnlyDictionary<int, EnemyData> IDToData => _idToData;
		public IReadOnlyDictionary<string, EnemyData> NameToData => _nameToData;

		public EnemyData GetData(int id)
		{
			if (_idToData.TryGetValue(id, out var data))
			{
				return data;
			}
			Debug.LogError($"EnemyData not found. ID: {id}");
			return null;
		}

		public EnemyData GetData(string name)
		{
			if (_nameToData.TryGetValue(name, out var data))
			{
				return data;
			}
			Debug.LogError($"EnemyData not found. Name: {name}");
			return null;
		}
	}

#if UNITY_EDITOR
	[UnityEditor.CustomEditor(typeof(EnemySheet))]
	public class EnemySheetDrawer : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Open Window"))
			{
				var sheet = target as EnemySheet;
				if (sheet != null)
				{
					EnemySheetWindow.ShowWindow(sheet);
				}
			}

			base.OnInspectorGUI();
		}

		[UnityEditor.Callbacks.OnOpenAsset]
		public static bool OnOpenAsset(int instanceId, int line)
		{
			if (UnityEditor.Selection.activeObject is EnemySheet sheet)
			{
				EnemySheetWindow.ShowWindow(sheet);
				return true;
			}
			return false;
		}
	}
#endif
}