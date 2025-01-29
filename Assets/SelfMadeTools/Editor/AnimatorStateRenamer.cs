using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace NexEditor
{
    public class AnimatorStateRenamer : EditorWindow
    {
        // --- 置換系 ---
        private string oldString = "";
        private string newString = "";

        // --- 挿入系 ---
        // 例: insertIndex = 2, insertString = "ABC" とすると、名前の2文字目に"ABC"を挿入
        private int insertIndex = 0;
        private string insertString = "";

        [MenuItem("Tools/Self Made Tools/Animator/Batch Rename")]
        private static void OpenWindow()
        {
            var window = GetWindow<AnimatorStateRenamer>("Animator Renamer");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("【一括置換】", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("検索文字列:", GUILayout.Width(80));
                oldString = EditorGUILayout.TextField(oldString);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("置換文字列:", GUILayout.Width(80));
                newString = EditorGUILayout.TextField(newString);
            }

            if (GUILayout.Button("選択中ステート名を置換"))
            {
                ReplaceStringInSelectedStates();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("【文字挿入】", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("挿入開始位置:", GUILayout.Width(80));
                insertIndex = EditorGUILayout.IntField(insertIndex);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("挿入文字列:", GUILayout.Width(80));
                insertString = EditorGUILayout.TextField(insertString);
            }

            if (GUILayout.Button("選択中ステート名に文字を挿入"))
            {
                InsertStringInSelectedStates();
            }
        }

        /// <summary>
        /// 選択中ステート/トランジションの名称の特定文字列を置き換える
        /// </summary>
        private void ReplaceStringInSelectedStates()
        {
            // 選択中のすべてのオブジェクトを取得
            var selectedObjects = Selection.objects;
            if (selectedObjects == null || selectedObjects.Length == 0)
            {
                Debug.LogWarning("オブジェクトが選択されていません。");
                return;
            }

            Undo.RecordObjects(selectedObjects, "Animator State Rename - Replace");

            foreach (var obj in selectedObjects)
            {
                // AnimatorState に対する処理
                if (obj is AnimatorState animatorState)
                {
                    RenameAnimatorState(animatorState, oldString, newString);
                }
                // AnimatorTransition に対する処理
                else if (obj is AnimatorStateTransition transition)
                {
                    RenameAnimatorTransition(transition, oldString, newString);
                }
            }

            // 変更を反映
            AssetDatabase.SaveAssets();
            Debug.Log("置換が完了しました。");
        }

        /// <summary>
        /// 選択中ステート/トランジションの名称に特定の位置から文字を挿入する
        /// </summary>
        private void InsertStringInSelectedStates()
        {
            var selectedObjects = Selection.objects;
            if (selectedObjects == null || selectedObjects.Length == 0)
            {
                Debug.LogWarning("オブジェクトが選択されていません。");
                return;
            }

            Undo.RecordObjects(selectedObjects, "Animator State Rename - Insert");

            foreach (var obj in selectedObjects)
            {
                // AnimatorState
                if (obj is AnimatorState animatorState)
                {
                    InsertToAnimatorState(animatorState, insertIndex, insertString);
                }
                // AnimatorTransition
                else if (obj is AnimatorStateTransition transition)
                {
                    InsertToAnimatorTransition(transition, insertIndex, insertString);
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log("文字挿入が完了しました。");
        }

        /// <summary>
        /// AnimatorState の名前を置換
        /// </summary>
        private void RenameAnimatorState(AnimatorState animatorState, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                // oldStr が空の場合は何もしない
                return;
            }

            var originalName = animatorState.name;
            var replaced = originalName.Replace(oldStr, newStr);
            if (originalName != replaced)
            {
                animatorState.name = replaced;
                EditorUtility.SetDirty(animatorState);
            }
        }

        /// <summary>
        /// AnimatorTransition の名前を置換
        /// </summary>
        private void RenameAnimatorTransition(AnimatorStateTransition transition, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return;
            }

            var originalName = transition.name;
            var replaced = originalName.Replace(oldStr, newStr);
            if (originalName != replaced)
            {
                transition.name = replaced;
                EditorUtility.SetDirty(transition);
            }
        }

        /// <summary>
        /// AnimatorState に文字列を挿入
        /// </summary>
        private void InsertToAnimatorState(AnimatorState animatorState, int index, string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var originalName = animatorState.name;
            var inserted = InsertString(originalName, index, text);
            if (originalName != inserted)
            {
                animatorState.name = inserted;
                EditorUtility.SetDirty(animatorState);
            }
        }

        /// <summary>
        /// AnimatorTransition に文字列を挿入
        /// </summary>
        private void InsertToAnimatorTransition(AnimatorStateTransition transition, int index, string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var originalName = transition.name;
            var inserted = InsertString(originalName, index, text);
            if (originalName != inserted)
            {
                transition.name = inserted;
                EditorUtility.SetDirty(transition);
            }
        }

        /// <summary>
        /// 文字列に対して特定のインデックスに文字を挿入する
        /// インデックスが範囲外の場合は、最も近い有効な範囲に挿入する
        /// </summary>
        private string InsertString(string original, int index, string text)
        {
            if (string.IsNullOrEmpty(original)) return text;
            if (index < 0) index = 0;
            if (index > original.Length) index = original.Length;

            return original.Substring(0, index) + text + original.Substring(index);
        }
    }
}