#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NexEditor.GameDataSheet
{
    [Serializable]
    public class WindowLayout<DataType> where DataType : ScriptableObject
    {
        private Vector2 _scrollPosition;

        private List<float> _labelWidths;
        private List<float> _valueWidths;
        private List<float> _heights;

        private DataType _pendingDeletion = null;

        public void Draw(SheetBase<DataType> sheet)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            CalcWidth(sheet);

            int indexY = 0;
            foreach (var data in sheet.Collection)
            {
                EditorGUILayout.BeginHorizontal();
                DrawRow(sheet, data, indexY); indexY++;
                EditorGUILayout.EndHorizontal();
            }

            DrawCreateButton(sheet);
            DrawGridLine();

            if (_pendingDeletion != null)
            {
                sheet.Delete(_pendingDeletion);
                _pendingDeletion = null;
            }

            EditorGUILayout.EndScrollView();
        }

        private void CalcWidth(SheetBase<DataType> sheet)
        {
            if (sheet == null || sheet.Collection == null) return;

            _labelWidths = new List<float>();
            _valueWidths = new List<float>();
            _heights = new List<float>();

            // Calculate label width
            {
                var firstData = sheet.Collection.FirstOrDefault();
                if (firstData == null) return;
                var serializedObject = new SerializedObject(firstData);
                serializedObject.Update();
                var iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);
                while (iterator.NextVisible(false))
                {
                    var width = new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(iterator.displayName)).x;
                    _labelWidths.Add(width);
                }
            }

            // Calculate value width and height
            var indexY = 0;
            foreach (var row in sheet.Collection)
            {
                var serializedObject = new SerializedObject(row);
                serializedObject.Update();
                var column = serializedObject.GetIterator();
                column.NextVisible(true);

                int indexX = 0;
                while (column.NextVisible(false))
                {
                    var size = column.GetSize();

                    if (_valueWidths.Count <= indexX) _valueWidths.Add(size.x);
                    else _valueWidths[indexX] = Mathf.Max(_valueWidths[indexX], size.x);

                    if (_heights.Count <= indexY) _heights.Add(size.y);
                    else _heights[indexY] = Mathf.Max(_heights[indexY], size.y);

                    indexX++;
                }
                indexY++;
            }
        }

        private void DrawRow(SheetBase<DataType> sheet, DataType data, int indexY)
        {
            var serializedObject = new SerializedObject(data);
            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);

            int indexX = 0;
            while (iterator.NextVisible(false))
            {
                DrawElement(iterator, indexY, indexX);
                indexX++;
            }

            serializedObject.ApplyModifiedProperties();

            var height = _heights.Count > indexY ? _heights[indexY] : 20f;
            if (GUILayout.Button("Delete", GUILayout.Width(120f), GUILayout.Height(height)))
            {
                _pendingDeletion = data;
            }
        }

        private void DrawElement(SerializedProperty iterator, int indexY, int indexX)
        {
            var height = _heights[indexY];
            var width = _labelWidths[indexX];
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft };
            EditorGUILayout.LabelField(iterator.displayName, style, GUILayout.Width(width), GUILayout.Height(height));

            width = _valueWidths[indexX];
            if (iterator.propertyType == SerializedPropertyType.String)
            {
                iterator.stringValue = EditorGUILayout.TextArea(iterator.stringValue, GUILayout.Width(width), GUILayout.Height(height));
            }
            else if (iterator.IsSprite())
            {
                // Sprite型のObjectFieldを描画
                iterator.objectReferenceValue = EditorGUILayout.ObjectField(iterator.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(width), GUILayout.Height(height));
            }
            else
            {
                EditorGUILayout.PropertyField(iterator, GUIContent.none, GUILayout.Width(width), GUILayout.Height(height));
            }
        }

        private void DrawCreateButton(SheetBase<DataType> sheet)
        {
            var width = _labelWidths.Sum() + _valueWidths.Sum() + 120f;
            width += _labelWidths.Count * 3f + _valueWidths.Count * 3f;
            if (GUILayout.Button("Create", GUILayout.Width(width)))
            {
                sheet.Create();
            }
        }

        private void DrawGridLine()
        {
            if (_labelWidths == null || _valueWidths == null || _heights == null) return;

            var offset = new Vector2(1, 1);
            var padding = new Vector2(3f, 2f);
            var color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            var totalLabelWidth = _labelWidths.Sum() + _labelWidths.Count * padding.x;
            var totalValueWidth = _valueWidths.Sum() + _valueWidths.Count * padding.x;
            var width = totalLabelWidth + totalValueWidth + 122f;
            var height = _heights.Sum() + _heights.Count * padding.y;

            // Draw horizontal lines
            for (int i = 0; i < _heights.Count; i++)
            {
                var y = _heights.Take(i).Sum() + i * padding.y + offset.y;
                EditorGUI.DrawRect(new Rect(offset.x, y, width, 1), color);
            }
            EditorGUI.DrawRect(new Rect(offset.x, height + offset.y, width, 1), color);

            // Draw vertical lines
            for (int i = 0; i < _labelWidths.Count; i++)
            {
                // label幅までの合計
                var lx = _labelWidths.Take(i).Sum() + i * padding.x;
                // value幅までの合計
                var vx = _valueWidths.Take(i).Sum() + i * padding.x;
                var x = lx + vx + offset.x;
                EditorGUI.DrawRect(new Rect(x, offset.y, 1, height), color);
            }
            EditorGUI.DrawRect(new Rect(width + offset.x - 122f, offset.y, 1, height), color);
            EditorGUI.DrawRect(new Rect(width + offset.x, offset.y, 1, height), color);
        }

    }
}
#endif