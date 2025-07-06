#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace NexEditor.ScriptableDashboard
{
    public class MakeDashboardWindow : EditorWindow
    {
        [MenuItem("Window/Make Scriptable Dashboard")]
        [MenuItem("Assets/Create/Scriptable Dashboard/New Scriptable Dashboard", false, 0)]
        static void Init()
        {
            GetWindow(typeof(MakeDashboardWindow)).Show();
        }

        private string _common = "";
        private static string _namespace = "";
        private static string _dataName = "Data";
        private static string _dashboardName = "Dashboard";
        private static string _windowName = "DashboardWindow";
        private static string _path;

        private static bool _autoCrate = true;

        private readonly static string _autoCrateKey = "AutoCrate";

        private readonly static string _pathKey = "AssetPath";
        private readonly static string _dashboardNameKey = "DashboardName";
        private readonly static string _windowNameKey = "WindowName";

        private void OnEnable()
        {
            _common = "";
            _namespace = "";
            _dataName = "Data";
            _dashboardName = "Dashboard";
            _windowName = "DashboardWindow";

            var rect = this.position;
            rect.width = 640f;
            rect.height = 240f;
            this.position = rect;
        }

        private void OnGUI()
        {
            // タイトル
            EditorGUILayout.LabelField("Make Dashboard Window");
            // CommonValue入力
            InputCommonValue();
            // Namespace入力フィールド
            InputFileName("Namespace: ", ref _namespace, true);
            // DataName入力フィールド
            InputFileName("Data Name: ", ref _dataName);
            // DashboardName入力フィールド
            InputFileName("Dashboard Name: ", ref _dashboardName);
            // WindowName入力フィールド
            InputFileName("Window Name: ", ref _windowName);
            // Path入力フィールド
            InputPath();
            // CSharpファイル生成後、自動でアセットを生成するかどうか。
            EditorGUILayout.BeginHorizontal();
            _autoCrate = EditorGUILayout.Toggle(_autoCrate, GUILayout.Width(15f));
            EditorGUILayout.LabelField("Generate dashboard assets automatically after recompilation is complete.");
            EditorGUILayout.EndHorizontal();

            // Makeフィールド
            InputMakeButton();
        }

        private void InputCommonValue()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Common", GUILayout.Width(108f));

            var emptyLabel = new GUIContent();
            var old = _common;
            _common = EditorGUILayout.TextField(emptyLabel, _common);
            if (old != _common)
            {
                _namespace = _common;
                _dataName = _common + "Data";
                _dashboardName = _common + "Dashboard";
                _windowName = _common + "DashboardWindow";
                _path = _common;
            }

            EditorGUILayout.LabelField("", GUILayout.Width(25f));

            EditorGUILayout.EndHorizontal();
        }

        private void InputFileName(string label, ref string fileName, bool hideFileExtension = false)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(label, GUILayout.Width(108f));

            var emptyLabel = new GUIContent();
            fileName = EditorGUILayout.TextField(emptyLabel, fileName);

            if (!hideFileExtension) EditorGUILayout.LabelField(".cs", GUILayout.Width(25f));
            else EditorGUILayout.LabelField("", GUILayout.Width(25f));

            EditorGUILayout.EndHorizontal();
        }

        private void InputPath()
        {
            EditorGUILayout.BeginHorizontal();

            GUIStyle labelStyle = GUI.skin.label;
            GUIContent labelContent = new GUIContent($"Path: {Application.dataPath}/");
            Vector2 labelSize = labelStyle.CalcSize(labelContent);


            EditorGUILayout.LabelField($"Path: {Application.dataPath}/", GUILayout.Width(labelSize.x));
            _path = EditorGUILayout.TextField(_path);
            EditorGUILayout.LabelField("", GUILayout.Width(25f));

            EditorGUILayout.EndHorizontal();
        }

        private string AdjustedDataPath => Application.dataPath + "\\" + _path + $"\\{_dataName}.cs";
        private string AdjustedDashboardPath => Application.dataPath + "\\" + _path + $"\\{_dashboardName}.cs";
        private string AdjustedWindowPath => Application.dataPath + "\\" + _path + $"\\{_windowName}.cs";

        private void InputMakeButton()
        {
            if (GUILayout.Button("Make"))
            {
                // Make DataName.cs file.
                FileCreator.MakeCSharpFile(AdjustedDataPath, FileTemplate.Data(_dataName, _namespace));
                // Make DashboardName.cs file.
                FileCreator.MakeCSharpFile(AdjustedDashboardPath, FileTemplate.Dashboard(_dashboardName, _dataName, _namespace));
                // Make WindowName.cs file.
                FileCreator.MakeCSharpFile(AdjustedWindowPath, FileTemplate.Window(_windowName, _dataName, _dashboardName, _namespace));

                // Save
                EditorPrefs.SetBool(_autoCrateKey, _autoCrate);
                EditorPrefs.SetString(_pathKey, _path);
                EditorPrefs.SetString(_dashboardNameKey, _dashboardName);
                EditorPrefs.SetString(_windowNameKey, _windowName);

                AssetDatabase.Refresh();
                Close();
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            var autoMake = EditorPrefs.GetBool(_autoCrateKey);
            if (autoMake)
            {
                EditorPrefs.SetBool(_autoCrateKey, false);
                var dashboardName = EditorPrefs.GetString(_dashboardNameKey);
                var path = Path.Combine("Assets", EditorPrefs.GetString(_pathKey));

                var instance = ScriptableObject.CreateInstance(dashboardName);

                var fileName = dashboardName + ".asset";
                if (instance) AssetDatabase.CreateAsset(instance, Path.Combine(path, fileName));
            }
        }
    }
}
#endif