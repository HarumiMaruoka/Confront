using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.ComponentModel;

namespace NexEditor.GameDataSheet
{
    public static class SerializedPropertyExtensions
    {
        public static string GetValueString(this SerializedProperty serializedProperty)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return serializedProperty.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return "0";
                case SerializedPropertyType.Float:
                    return serializedProperty.floatValue.ToString();
                case SerializedPropertyType.String:
                    return serializedProperty.stringValue;
                case SerializedPropertyType.Color:
                    return "00000000";
                case SerializedPropertyType.ObjectReference:
                    return serializedProperty.objectReferenceValue != null
                        ? serializedProperty.objectReferenceValue.ToString()
                        : "000000000000000000";
                case SerializedPropertyType.LayerMask:
                    return serializedProperty.intValue.ToString();
                case SerializedPropertyType.Enum:
                    return serializedProperty.enumDisplayNames[serializedProperty.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return serializedProperty.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return serializedProperty.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return serializedProperty.vector4Value.ToString();
                case SerializedPropertyType.Rect:
                    return serializedProperty.rectValue.ToString();
                case SerializedPropertyType.ArraySize:
                    return serializedProperty.arraySize.ToString();
                case SerializedPropertyType.Character:
                    return serializedProperty.intValue.ToString();
                case SerializedPropertyType.AnimationCurve:
                    return "000000";
                case SerializedPropertyType.Bounds:
                    return serializedProperty.boundsValue.ToString();
                case SerializedPropertyType.Quaternion:
                    return serializedProperty.quaternionValue.ToString();
                case SerializedPropertyType.ExposedReference:
                    return serializedProperty.exposedReferenceValue != null
                        ? serializedProperty.exposedReferenceValue.ToString()
                        : "null";
                case SerializedPropertyType.FixedBufferSize:
                    return serializedProperty.fixedBufferSize.ToString();
                case SerializedPropertyType.Vector2Int:
                    return serializedProperty.vector2IntValue.ToString();
                case SerializedPropertyType.Vector3Int:
                    return serializedProperty.vector3IntValue.ToString();
                case SerializedPropertyType.RectInt:
                    return serializedProperty.rectIntValue.ToString();
                case SerializedPropertyType.BoundsInt:
                    return serializedProperty.boundsIntValue.ToString();
                case SerializedPropertyType.Generic:
                    return serializedProperty.ToString();
                default:
                    return string.Empty;
            }
        }

        public static Vector2 GetSize(this SerializedProperty serializedProperty)
        {
            var padding = new Vector2(8f, 6f);

            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.String:
                    return new GUIStyle(GUI.skin.textArea).CalcSize(new GUIContent(serializedProperty.stringValue)) + padding;
                default:
                    return new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(serializedProperty.GetValueString())) + padding;
            }
        }
    }
}