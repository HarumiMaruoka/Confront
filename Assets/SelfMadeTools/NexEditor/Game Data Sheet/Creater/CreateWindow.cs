#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace NexEditor.GameDataSheet
{
    public class CreateWindow : EditorWindow
    {
        [MenuItem("Window/Game Data Sheet Create Window")]
        [MenuItem("Assets/Create/Game Data Sheet", false, 0)]
        static void Init()
        {
            GetWindow(typeof(CreateWindow)).Show();
        }


        private string _common = "";
        private static string _namespace = "";
        private static string _dataName = "Data";
        private static string _sheetName = "Sheet";
        private static string _windowLayout = "SheetWindowLayout";
        private static string _windowName = "SheetWindow";
        private static string _path;

        private static bool _autoCrate = true;
        private static bool _autoShow = true;

        private readonly static string _autoCrateKey = "AutoCrate";
        private readonly static string _autoShowKey = "AutoShow";

        private readonly static string _pathKey = "AssetPath";
        private readonly static string _sheetNameKey = "SheetName";
        private readonly static string _windowNameKey = "WindowName";

        private void OnEnable()
        {
            _common = "";
            _namespace = "";
            _dataName = "Data";
            _sheetName = "Sheet";
            _windowLayout = "SheetWindowLayout";
            _windowName = "SheetWindow";

            var rect = this.position;
            rect.width = 640f;
            rect.height = 240f;
            this.position = rect;
        }

        private void OnGUI()
        {
            // タイトル
            EditorGUILayout.LabelField("Sheet Create Window");
            // CommonValue入力
            InputCommonValue();
            // Namespace入力フィールド
            InputFileName("Namespace: ", ref _namespace, true);
            // DataName入力フィールド
            InputFileName("Data Name: ", ref _dataName);
            // SheetName入力フィールド
            InputFileName("Sheet Name: ", ref _sheetName);
            // WindowLayout入力フィールド
            InputFileName("Window Layout: ", ref _windowLayout);
            // WindowName入力フィールド
            InputFileName("Window Name: ", ref _windowName);
            // Path入力フィールド
            InputPath();
            // CSharpファイル生成後、自動でアセットを生成するかどうか。
            EditorGUILayout.BeginHorizontal();
            _autoCrate = EditorGUILayout.Toggle(_autoCrate, GUILayout.Width(15f));
            EditorGUILayout.LabelField("Generate sheet assets automatically after recompilation is complete.");
            EditorGUILayout.EndHorizontal();

            // CSharpファイル生成後、自動でウィンドウを開くかどうか。
            EditorGUILayout.BeginHorizontal();
            _autoShow = EditorGUILayout.Toggle(_autoShow, GUILayout.Width(15f));
            EditorGUILayout.LabelField("Display the sheet window automatically after recompilation is complete.");
            EditorGUILayout.EndHorizontal();

            // Createフィールド
            InputCreateButton();
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
                _sheetName = _common + "Sheet";
                _windowLayout = _common + "SheetWindowLayout";
                _windowName = _common + "SheetWindow";
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
        private string AdjustedSheetPath => Application.dataPath + "\\" + _path + $"\\{_sheetName}.cs";
        private string AdjustedLayoutPath => Application.dataPath + "\\" + _path + $"\\{_windowLayout}.cs";
        private string AdjustedWindowPath => Application.dataPath + "\\" + _path + $"\\{_windowName}.cs";

        private void InputCreateButton()
        {
            if (GUILayout.Button("Create"))
            {
                // Create DataName.cs file.
                FileCreator.CreateCSharpFile(AdjustedDataPath, FileTemplate.Data(_dataName, _namespace));
                // Create SheetName.cs file.
                FileCreator.CreateCSharpFile(AdjustedSheetPath, FileTemplate.Sheet(_sheetName, _dataName, _windowName, _namespace));
                // Create WindowLayout.cs file.
                FileCreator.CreateCSharpFile(AdjustedLayoutPath, FileTemplate.WindowLayout(_windowLayout, _dataName, _namespace));
                // Create WindowName.cs file.
                FileCreator.CreateCSharpFile(AdjustedWindowPath, FileTemplate.Window(_windowName, _dataName, _sheetName, _windowLayout, _namespace));

                // Save
                EditorPrefs.SetBool(_autoCrateKey, _autoCrate);
                EditorPrefs.SetBool(_autoShowKey, _autoShow);
                EditorPrefs.SetString(_pathKey, _path);
                EditorPrefs.SetString(_sheetNameKey, _sheetName);
                EditorPrefs.SetString(_windowNameKey, _windowName);

                AssetDatabase.Refresh();
                Close();
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            var autoCreate = EditorPrefs.GetBool(_autoCrateKey);
            if (autoCreate)
            {
                EditorPrefs.SetBool(_autoCrateKey, false);
                var sheetName = EditorPrefs.GetString(_sheetNameKey);
                var path = Path.Combine("Assets", EditorPrefs.GetString(_pathKey));

                var instance = ScriptableObject.CreateInstance(sheetName);

                var fileName = sheetName + ".asset";
                if (instance) AssetDatabase.CreateAsset(instance, Path.Combine(path, fileName));
            }

            var autoShow = EditorPrefs.GetBool(_autoShowKey);
            if (autoShow)
            {
                EditorPrefs.SetBool(_autoShowKey, false);
                var windowTypeName = EditorPrefs.GetString(_windowNameKey);
                EditorApplication.ExecuteMenuItem($"Window/Game Data Sheet/{windowTypeName}");
            }
        }
    }
}
#endif