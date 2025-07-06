#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NexEditor.ScriptableDashboard
{
    public abstract class ScriptableDashboardEditor<DataType> : EditorWindow where DataType : ScriptableObject
    {
        // === UXML assets ===
        private static VisualTreeAsset visualTreeAsset = default;

        // === Runtime references ===
        private ScriptableDashboard<DataType> _dashboard;
        private readonly Dictionary<DataType, SerializedObject> _serializedObjects = new();

        // === UI elements ===
        private ObjectField _objectField;

        private Button _addButton;
        private Button _removeButton;
        private Button _insertButton;

        private MultiColumnListView _table;
        private MaskField _filterMaskField;
        private TextField _filterField;

        // === Reflection cache ===
        private readonly List<string> _fieldNames = new();
        private FieldInfo[] _fieldInfos;

        // ----------------------------------------------------------------------
        // Public entry points
        // ----------------------------------------------------------------------

        public void Setup(ScriptableDashboard<DataType> dashboard)
        {
            _objectField.value = dashboard;
            ReloadList();
            RefreshSerializedObjects();
        }

        private void OnEnable()
        {
            SetupVisualTreeAsset();
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        private void CreateGUI()
        {
            // UXMLの読み込み
            var root = rootVisualElement;
            visualTreeAsset.CloneTree(root);

            // 編集対象のダッシュボードを設定
            _objectField = root.Q<ObjectField>("ObjectField");
            if (_objectField != null)
            {
                _objectField.objectType = typeof(ScriptableDashboard<DataType>);
                _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            }

            // ボタン
            _addButton = root.Q<Button>("AddButton");
            if (_addButton != null) _addButton.clicked += OnAddButtonClicked;
            _removeButton = root.Q<Button>("RemoveButton");
            if (_removeButton != null) _removeButton.clicked += OnRemoveButtonClicked;
            _insertButton = root.Q<Button>("InsertButton");
            if (_insertButton != null) _insertButton.clicked += OnInsertButtonClicked;

            // テーブル
            _table = root.Q<MultiColumnListView>("TableView");
            InitTableView(_table);

            InitializeFieldInfos();
            BuildColumns();

            // フィルタ
            _filterMaskField = root.Q<MaskField>("FilterMaskField");
            _filterMaskField.RegisterValueChangedCallback(_ => ReloadList());
            _filterField = root.Q<TextField>("FilterTextField");
            _filterField.RegisterValueChangedCallback(_ => ReloadList());

            _filterMaskField.choices = _fieldNames.Select(ObjectNames.NicifyVariableName).ToList();
            _filterMaskField.value = -1;

            // イベントハンドラ
            _table.itemIndexChanged += (_, _) =>
            {
                Undo.RecordObject(_dashboard, "Reorder Dashboard");
                EditorUtility.SetDirty(_dashboard);
                AssetDatabase.SaveAssets();
            };

            _table.columnSortingChanged += OnColumnSortingChanged;

            // ダッシュボードの設定
            ReloadList();
        }

        private void SetupVisualTreeAsset()
        {
            if (visualTreeAsset == null)
            {
                string[] guids = AssetDatabase.FindAssets("ScriptableDashboardEditor t:VisualTreeAsset");

                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
                }

                if (visualTreeAsset == null)
                {
                    Debug.LogError("VisualTreeAsset not found for ScriptableDashboardEditor.");
                    return;
                }
            }
        }

        // ----------------------------------------------------------------------
        //  Internal helpers
        // ----------------------------------------------------------------------

        private void InitTableView(MultiColumnListView table)
        {
            table.name = "DashboardTable";
            table.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
            table.selectionType = SelectionType.Multiple;
            table.reorderable = true;
            table.reorderMode = ListViewReorderMode.Animated;
            table.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            table.fixedItemHeight = 18;
            table.sortingMode = ColumnSortingMode.Default;
        }

        private void InitializeFieldInfos()
        {
            var type = typeof(DataType);
            _fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            _fieldNames.Clear();
            _fieldNames.AddRange(_fieldInfos.Select(f => f.Name));
        }

        private void BuildColumns()
        {
            _table.columns.Clear();
            _table.columns.stretchMode = Columns.StretchMode.Grow;
            _table.horizontalScrollingEnabled = true;
            _table.sortingMode = ColumnSortingMode.Default;

            for (int col = 0; col < _fieldInfos.Length; col++)
            {
                FieldInfo fieldInfo = _fieldInfos[col];
                int capturedCol = col;

                _table.columns.Add(new Column
                {
                    title = ObjectNames.NicifyVariableName(fieldInfo.Name),
                    resizable = true,
                    sortable = true,

                    width = 150,
                    minWidth = 50,
                    stretchable = false,

                    comparison = (a, b) =>
                    {
                        var dataA = (DataType)_table.itemsSource[a];
                        var dataB = (DataType)_table.itemsSource[b];
                        var valueA = fieldInfo.GetValue(dataA);
                        var valueB = fieldInfo.GetValue(dataB);
                        if (valueA == null && valueB == null) return 0;
                        if (valueA == null) return -1;
                        if (valueB == null) return 1;
                        return string.Compare(valueA.ToString(), valueB.ToString(), System.StringComparison.OrdinalIgnoreCase);
                    },

                    makeCell = () => new PropertyField { name = $"cell_{capturedCol}", label = string.Empty },

                    bindCell = (visualElement, rowIndex) =>
                    {
                        var data = (DataType)_table.itemsSource[rowIndex];

                        if (!_serializedObjects.TryGetValue(data, out var serializedObject))
                        {
                            serializedObject = new SerializedObject(data);
                            _serializedObjects[data] = serializedObject;
                        }

                        serializedObject.Update();
                        var pf = (PropertyField)visualElement;
                        var prop = serializedObject.FindProperty(fieldInfo.Name);
                        pf.Unbind();
                        pf.BindProperty(prop);
                    }
                });
            }
        }

        private void ReloadList()
        {
            if (_dashboard == null) return;

            // 現在のマスク・キーワード取得
            int mask = _filterMaskField?.value ?? -1;
            bool allMask = mask == -1; // -1 は「全列」
            string keyword = _filterField?.value ?? "";
            keyword = keyword.Trim();

            // キーワードが空なら全アイテムを表示し、return。
            if (string.IsNullOrEmpty(keyword))
            {
                _table.itemsSource = _dashboard.Collection;
                _table.Rebuild();
                return;
            }

            // キーワードがある場合は、フィルタリングを行う。
            var result = new List<DataType>();
            foreach (var item in _dashboard.Collection)
            {
                for (int col = 0; col < _fieldInfos.Length; col++)
                {
                    if (!allMask && (mask & (1 << col)) == 0) continue; // 無効な列

                    object value = _fieldInfos[col].GetValue(item);
                    if (value == null) continue;

                    if (value.ToString().IndexOf(keyword, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result.Add(item);
                        break; // 1列でもマッチすれば OK
                    }
                }
            }

            _table.itemsSource = result;
            _table.Rebuild();
        }

        private void RefreshSerializedObjects()
        {
            if (_dashboard == null) return;

            // 現在のダッシュボードのコレクションに基づいて SerializedObject を更新
            foreach (var obj in _dashboard.Collection)
            {
                if (!_serializedObjects.ContainsKey(obj))
                    _serializedObjects[obj] = new SerializedObject(obj);
            }

            // ダッシュボードのコレクションに存在しない SerializedObject を削除
            var staleKeys = _serializedObjects.Keys.Except(_dashboard.Collection).ToList();
            foreach (var key in staleKeys)
                _serializedObjects.Remove(key);
        }

        private void OnUndoRedo()
        {
            if (_table != null)
            {
                EditorApplication.delayCall += _table.Rebuild;
            }
        }

        private void OnObjectFieldValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            _dashboard = evt.newValue as ScriptableDashboard<DataType>;
            ReloadList();
            RefreshSerializedObjects();
        }

        private void OnAddButtonClicked()
        {
            if (_dashboard == null) return;

            var newItem = _dashboard.Create();
            _serializedObjects[newItem] = new SerializedObject(newItem);

            _table.ClearSelection();
            _table.Rebuild();
        }

        private void OnRemoveButtonClicked()
        {
            if (_dashboard == null) return;

            foreach (var item in _table.selectedItems)
                _dashboard.Delete((DataType)item);

            _table.ClearSelection();
            _table.Rebuild();
        }

        private void OnInsertButtonClicked()
        {
            if (_dashboard == null) return;

            // MultiColumnListView は selectedIndices が昇順保証なので「後ろから」挿入してインデックスずれを回避。
            foreach (var index in _table.selectedIndices.Reverse())
            {
                // 挿入位置は選択された行の次の位置
                int insertIndex = index + 1;
                // 新しいアイテムを作成
                var newItem = _dashboard.CreateAndInsert(insertIndex);
                if (newItem != null)
                {
                    _serializedObjects[newItem] = new SerializedObject(newItem);
                }
            }

            _table.ClearSelection();
            _table.Rebuild();
        }

        private void OnColumnSortingChanged()
        {
            _insertButton.SetEnabled(!IsTableSorted());
        }

        private bool IsTableSorted()
        {
            return _table != null && _table.sortedColumns != null && _table.sortedColumns.Any();
        }
    }
}
#endif