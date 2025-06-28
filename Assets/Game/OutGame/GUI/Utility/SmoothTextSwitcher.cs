using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.GUI
{
    public class SmoothTextSwitcher : TMPro.TextMeshProUGUI
    {
        public float speed = 10f;

        private CancellationTokenSource _cancellationTokenSource;

        public async void SetText(string text)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;

                Color col = this.color;
                while (col.a > 0)
                {
                    if (token.IsCancellationRequested) return;

                    col.a -= Time.deltaTime * speed;
                    this.color = col;
                    await UniTask.Yield(token);
                }

                this.text = text;

                col = this.color;
                while (col.a <= 1)
                {
                    if (token.IsCancellationRequested) return;

                    col.a += Time.deltaTime * speed;
                    this.color = col;
                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SmoothTextSwitcher), true), UnityEditor.CanEditMultipleObjects]
    public class SmoothTextSwitcherEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
    {
        UnityEditor.SerializedProperty _speedProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _speedProperty = serializedObject.FindProperty("speed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // TextMeshProUGUIの標準インスペクタ表示を呼び出す
            base.OnInspectorGUI();

            UnityEditor.EditorGUILayout.Space();
            UnityEditor.EditorGUILayout.LabelField("Custom Properties", UnityEditor.EditorStyles.boldLabel);

            // カスタムプロパティ表示
            UnityEditor.EditorGUILayout.PropertyField(_speedProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}