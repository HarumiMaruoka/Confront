using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.GUI
{
    public class SmoothImageSwitcher : UnityEngine.UI.Image
    {
        public float speed = 10f;

        private CancellationTokenSource _cancellationTokenSource;

        public async void SetSprite(Sprite sprite)
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

                this.sprite = sprite;

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
    [UnityEditor.CustomEditor(typeof(SmoothImageSwitcher), true), UnityEditor.CanEditMultipleObjects]
    public class SmoothImageSwitcherEditor : UnityEditor.UI.ImageEditor
    {
        UnityEditor.SerializedProperty speedProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            speedProperty = serializedObject.FindProperty("speed");
        }

        public override void OnInspectorGUI()
        {
            // まず標準のImageインスペクターを描画
            base.OnInspectorGUI();

            serializedObject.Update();

            UnityEditor.EditorGUILayout.Space();
            UnityEditor.EditorGUILayout.LabelField("Custom Properties", UnityEditor.EditorStyles.boldLabel);
            UnityEditor.EditorGUILayout.PropertyField(speedProperty, new UnityEngine.GUIContent("Speed"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}