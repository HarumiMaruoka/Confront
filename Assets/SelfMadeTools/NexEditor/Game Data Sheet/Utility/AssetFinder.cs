#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace NexEditor.GameDataSheet
{
    public class AssetFinder
    {
        public static T FindAssetsByType<T>() where T : UnityEngine.Object
        {
            // 検索フィルタを使用してアセットを検索
            string filter = "t:" + typeof(T).Name;
            string[] guids = AssetDatabase.FindAssets(filter);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<T>(path); // 最初に見つかったやつを返す。
            }

            return null;
        }
    }
}
#endif