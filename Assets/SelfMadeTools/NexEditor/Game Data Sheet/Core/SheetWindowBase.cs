#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

namespace NexEditor.GameDataSheet
{
    public class SheetWindowBase<DataType, SheetType, WindowLayoutType> : EditorWindow
        where DataType : ScriptableObject
        where SheetType : SheetBase<DataType>
        where WindowLayoutType : WindowLayout<DataType>
    {
        private SheetBase<DataType> _sheet;
        private WindowLayout<DataType> _windowLayout;

        private void OnEnable()
        {
            _sheet = AssetFinder.FindAssetsByType<SheetType>();

            Undo.undoRedoPerformed += () =>
            {
                Show();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            };
        }

        void OnGUI()
        {
            _sheet = EditorGUILayout.ObjectField("Target", _sheet, typeof(SheetType), false) as SheetType;

            if (_sheet)
            {
                if (_windowLayout == null) _windowLayout = Activator.CreateInstance<WindowLayoutType>();

                _windowLayout.Draw(_sheet);
            }
        }
    }
}
#endif