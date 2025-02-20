using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace NexEditor
{
    public class MergeTexturesEditor : EditorWindow
    {
        // テクスチャのリスト（ドラッグ＆ドロップで追加）
        private List<Texture2D> textures = new List<Texture2D>();
        private Vector2 scrollPos;

        [MenuItem("Tools/Self Made Tools/Merge Textures")]
        static void Init()
        {
            MergeTexturesEditor window = (MergeTexturesEditor)EditorWindow.GetWindow(typeof(MergeTexturesEditor));
            window.titleContent = new GUIContent("テクスチャ結合");
            window.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("複数の画像を一つに結合", EditorStyles.boldLabel);

            // ドラッグ＆ドロップ領域の作成
            GUILayout.Space(10);
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "ここにテクスチャをドラッグ＆ドロップ");
            HandleDragAndDrop(dropArea);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("追加されたテクスチャ", EditorStyles.boldLabel);

            // 追加済みテクスチャの表示
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i] = (Texture2D)EditorGUILayout.ObjectField("テクスチャ " + (i + 1), textures[i], typeof(Texture2D), false);
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("結合して保存"))
            {
                MergeAndSaveTextures();
            }
        }

        /// <summary>
        /// ドラッグ＆ドロップでテクスチャをリストに追加
        /// </summary>
        /// <param name="dropArea">ドロップエリア</param>
        void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;
            if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
            {
                if (!dropArea.Contains(evt.mousePosition))
                    return;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        Texture2D tex = draggedObject as Texture2D;
                        if (tex != null && !textures.Contains(tex))
                        {
                            textures.Add(tex);
                        }
                    }
                }
                evt.Use();
            }
        }

        /// <summary>
        /// テクスチャを結合して PNG 形式で保存する
        /// 横幅が最大テクスチャサイズを超える場合はグリッドレイアウトで配置
        /// </summary>
        void MergeAndSaveTextures()
        {
            if (textures == null || textures.Count == 0)
            {
                Debug.LogError("テクスチャが一つも割り当てられていません！");
                return;
            }

            // 横連結時の合計幅と最大高さを計算
            int totalWidth = 0;
            int maxHeight = 0;
            foreach (var tex in textures)
            {
                if (tex != null)
                {
                    totalWidth += tex.width;
                    maxHeight = Mathf.Max(maxHeight, tex.height);
                }
            }

            int maxAllowedSize = SystemInfo.maxTextureSize;
            Texture2D mergedTex = null;

            if (totalWidth <= maxAllowedSize)
            {
                // 横に連結できる場合
                mergedTex = new Texture2D(totalWidth, maxHeight, TextureFormat.RGBA32, false);
                // 全体を透明に初期化
                Color[] fillColor = new Color[totalWidth * maxHeight];
                for (int i = 0; i < fillColor.Length; i++)
                {
                    fillColor[i] = Color.clear;
                }
                mergedTex.SetPixels(fillColor);

                int offsetX = 0;
                foreach (var tex in textures)
                {
                    if (tex != null)
                    {
                        Color[] pixels = tex.GetPixels();
                        mergedTex.SetPixels(offsetX, 0, tex.width, tex.height, pixels);
                        offsetX += tex.width;
                    }
                }
            }
            else
            {
                // 横幅が最大サイズを超える場合はグリッドレイアウトで配置
                // 各テクスチャの最大幅と最大高さをセルサイズとして取得
                int cellWidth = 0;
                int cellHeight = 0;
                foreach (var tex in textures)
                {
                    if (tex != null)
                    {
                        cellWidth = Mathf.Max(cellWidth, tex.width);
                        cellHeight = Mathf.Max(cellHeight, tex.height);
                    }
                }
                // 最大テクスチャサイズ内に収まる列数を計算（最低1列）
                int columns = Mathf.Max(1, maxAllowedSize / cellWidth);
                int rows = Mathf.CeilToInt((float)textures.Count / columns);
                int mergedWidth = columns * cellWidth;
                int mergedHeight = rows * cellHeight;
                mergedTex = new Texture2D(mergedWidth, mergedHeight, TextureFormat.RGBA32, false);
                // 全体を透明に初期化
                Color[] fillColor = new Color[mergedWidth * mergedHeight];
                for (int i = 0; i < fillColor.Length; i++)
                {
                    fillColor[i] = Color.clear;
                }
                mergedTex.SetPixels(fillColor);

                // グリッドレイアウトにテクスチャを配置
                for (int i = 0; i < textures.Count; i++)
                {
                    Texture2D tex = textures[i];
                    if (tex != null)
                    {
                        int col = i % columns;
                        int row = i / columns;  // row 0 が下段
                        int xPos = col * cellWidth;
                        int yPos = row * cellHeight;

                        // ※必要なら、各セル内で中央寄せにする例
                        // int offsetX = (cellWidth - tex.width) / 2;
                        // int offsetY = (cellHeight - tex.height) / 2;
                        // xPos += offsetX;
                        // yPos += offsetY;

                        Color[] pixels = tex.GetPixels();
                        mergedTex.SetPixels(xPos, yPos, tex.width, tex.height, pixels);
                    }
                }
            }

            mergedTex.Apply();

            // PNG形式で保存
            byte[] pngData = mergedTex.EncodeToPNG();
            if (pngData != null)
            {
                string path = EditorUtility.SaveFilePanel("結合画像を保存", "", "MergedTexture.png", "png");
                if (!string.IsNullOrEmpty(path))
                {
                    File.WriteAllBytes(path, pngData);
                    AssetDatabase.Refresh();
                    Debug.Log("結合画像を保存しました: " + path);
                }
            }
        }
    }
}