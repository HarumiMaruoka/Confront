using Confront.Player.Combo;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Confront.Player.ComboEditor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        private Editor _editor;
        private Vector2 _scrollPosition;

        public InspectorView()
        {

        }

        public void UpdateSelection(NodeView nodeView)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                _editor.serializedObject.Update();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                var serializedProperty = _editor.serializedObject.FindProperty("Behaviour");
                EditorGUILayout.PropertyField(serializedProperty);

                var node = _editor.target as ComboNode;
                if (node != null && node.Behaviour != null)
                {
                    var serializedObject = new SerializedObject(node.Behaviour);
                    serializedObject.Update();
                    var iterator = serializedObject.GetIterator();
                    iterator.NextVisible(true);
                    while (iterator.NextVisible(false))
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                    serializedObject.ApplyModifiedProperties();
                }

                if (_editor.serializedObject.ApplyModifiedProperties())
                {
                    nodeView.UpdateNodeTitle();
                }
                EditorGUILayout.EndScrollView();
            });
            Add(container);
        }
    }
}