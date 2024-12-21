#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

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
                    if (serializedProperty.IsSprite()) return "000000000000000";
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
                case SerializedPropertyType.ObjectReference:
                    if (serializedProperty.IsSprite()) return new Vector2(16f * 3.5f, 16f * 3f) + padding;
                    if (serializedProperty.objectReferenceValue == null) return new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(serializedProperty.type)) + new Vector2(20f, 6f);
                    return new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(serializedProperty.objectReferenceValue.name)) + new Vector2(20f, 6f);
                case SerializedPropertyType.String:
                    return new GUIStyle(GUI.skin.textArea).CalcSize(new GUIContent(serializedProperty.stringValue)) + padding;
                default:
                    return new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(serializedProperty.GetValueString())) + padding;
            }
        }

        public static bool IsSprite(this SerializedProperty property)
        {
            // SerializedPropertyがオブジェクト参照型かチェック
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // 参照しているオブジェクトを取得
                Object obj = property.objectReferenceValue;
                // そのオブジェクトがSpriteか判定
                return obj is Sprite;
            }

            return false;
        }

    }
}
#endif