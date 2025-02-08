using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace NexEditor
{
    /// <summary>
    /// ExpandableAttribute 用のカスタムプロパティドロワー
    /// ScriptableObject 参照フィールドと、折りたたみでその中身を表示
    /// </summary>
    [CustomPropertyDrawer(typeof(ExpandableAttribute))]
    public class ExpandableDrawer : PropertyDrawer
    {
        // フォールドアウト状態を保持するための辞書
        // キー: propertyPath, 値: bool (true=展開中)
        private static Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

        /// <summary>
        /// プロパティ描画の高さを返す
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // デフォルトの1行分（オブジェクト参照のフィールド分）
            float totalHeight = EditorGUIUtility.singleLineHeight;

            // ScriptableObject が入っていて、かつフォールドアウトが開いているなら、その中のプロパティ分の高さを足す
            if (property.objectReferenceValue != null)
            {
                string key = GetKey(property);
                bool isExpanded = false;
                if (_foldoutStates.TryGetValue(key, out bool foldout))
                {
                    isExpanded = foldout;
                }

                if (isExpanded)
                {
                    // ScriptableObject 内部の SerializedObject を取得して、シリアライズされているプロパティすべての高さを加算
                    SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                    SerializedProperty prop = serializedObject.GetIterator();

                    // 最初の要素に対しては MoveNext() 必要
                    if (prop.NextVisible(true))
                    {
                        // script フィールド等、不要であれば除外するロジックを入れる
                        do
                        {
                            if (prop.name.Equals("m_Script", System.StringComparison.Ordinal))
                                continue;

                            // 各プロパティ1行分を仮定（必要であればさらに計算する）
                            var height = EditorGUI.GetPropertyHeight(prop, null, true);
                            totalHeight += height + 2f;
                        }
                        while (prop.NextVisible(false));
                    }
                }
            }

            return totalHeight;
        }

        /// <summary>
        /// プロパティを描画
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 1行目: ScriptableObject 参照フィールド
            Rect foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            Rect objectFieldRect = new Rect(
                position.x + EditorGUIUtility.labelWidth,
                position.y,
                position.width - EditorGUIUtility.labelWidth,
                EditorGUIUtility.singleLineHeight
            );

            // フォールドアウト用のキー
            string key = GetKey(property);

            // 辞書から現在のフォールドアウト状態を取り出す
            bool isExpanded = false;
            if (_foldoutStates.TryGetValue(key, out bool foldout))
            {
                isExpanded = foldout;
            }

            // ScriptableObject の参照フィールドを描画
            EditorGUI.PropertyField(objectFieldRect, property, GUIContent.none);

            // ScriptableObject がセットされている場合のみ、フォールドアウトを描画する
            if (property.objectReferenceValue != null)
            {
                isExpanded = EditorGUI.Foldout(foldoutRect, isExpanded, label, true);

                // フォールドアウト状態を更新
                if (_foldoutStates.ContainsKey(key))
                {
                    _foldoutStates[key] = isExpanded;
                }
                else
                {
                    _foldoutStates.Add(key, isExpanded);
                }

                // フォールドアウトを開いているときに、ScriptableObject の中身を表示する
                if (isExpanded)
                {
                    EditorGUI.indentLevel++;
                    // 一つ下の行から中身を描画
                    Rect propPos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2f,
                        position.width, EditorGUIUtility.singleLineHeight);

                    SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                    SerializedProperty prop = serializedObject.GetIterator();

                    if (prop.NextVisible(true))
                    {
                        do
                        {
                            // m_Script フィールドなど不要なものはスキップ
                            if (prop.name.Equals("m_Script", System.StringComparison.Ordinal))
                                continue;

                            propPos.height = EditorGUI.GetPropertyHeight(prop, null, true);
                            EditorGUI.PropertyField(propPos, prop, true);
                            propPos.y += EditorGUI.GetPropertyHeight(prop, null, true) + 2f;
                        }
                        while (prop.NextVisible(false));
                    }

                    // ScriptableObject内の変更を反映
                    serializedObject.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                // ScriptableObject が null の場合は、通常ラベルで描画
                EditorGUI.LabelField(foldoutRect, label);
            }
        }

        /// <summary>
        /// フォールドアウトの辞書キーにするユニーク文字列を作成
        /// </summary>
        private string GetKey(SerializedProperty property)
        {
            if (property == null) return string.Empty;
            return property.serializedObject.targetObject.GetInstanceID() + "/" + property.propertyPath;
        }
    }
}