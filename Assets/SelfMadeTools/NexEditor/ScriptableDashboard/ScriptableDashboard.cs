using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NexEditor.ScriptableDashboard
{
    public class ScriptableDashboard<T> : ScriptableObject, IEnumerable<T> where T : ScriptableObject
    {
        [SerializeField]
        private List<T> _collection;
        public List<T> Collection => _collection ??= new List<T>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public T this[int i] => _collection[i];

        public int Count => _collection.Count;

        public T Create()
        {
            var instance = CreateInstance<T>();

#if UNITY_EDITOR
            // Undoの開始（親アセットの変更）
            Undo.RecordObject(this, "Update Dashboard");
#endif
            // コレクションに追加
            _collection.Add(instance);

#if UNITY_EDITOR
            // サブアセットとして登録
            AssetDatabase.AddObjectToAsset(instance, this);

            // サブアセット追加をUndoに登録
            Undo.RegisterCreatedObjectUndo(instance, "Create Asset");

            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            return instance;
        }

        public T[] Create(int count)
        {
            if (count <= 0) return Array.Empty<T>();
            var instances = new T[count];

#if UNITY_EDITOR
            // Undoの開始（親アセットの変更）
            Undo.RecordObject(this, "Update Dashboard");
#endif
            for (int i = 0; i < count; i++)
            {
                var instance = CreateInstance<T>();
                _collection.Add(instance);
                instances[i] = instance;
#if UNITY_EDITOR
                // サブアセットとして登録
                AssetDatabase.AddObjectToAsset(instance, this);
                // サブアセット追加をUndoに登録
                Undo.RegisterCreatedObjectUndo(instance, "Create Asset");
            }

            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

            return instances;
        }

        public T CreateAndInsert(int index)
        {
            if (index < 0) index = 0;
            else if (index > _collection.Count) return Create();

            var instance = CreateInstance<T>();
#if UNITY_EDITOR
            // Undoの開始（親アセットの変更）
            Undo.RecordObject(this, "Update Dashboard");
#endif
            // コレクションに追加
            _collection.Insert(index, instance);
#if UNITY_EDITOR
            // サブアセットとして登録
            AssetDatabase.AddObjectToAsset(instance, this);
            // サブアセット追加をUndoに登録
            Undo.RegisterCreatedObjectUndo(instance, "Create Asset");
            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            return instance;
        }

        public void CreateAndInsert(SortedSet<int> indexes)
        {
            if (indexes == null || indexes.Count == 0) return;
            foreach (var item in indexes.Reverse()) CreateAndInsert(item + 1);
        }

        public void Delete(T data)
        {
#if UNITY_EDITOR
            // アセットの変更をUndoに登録
            Undo.RecordObject(this, "Delete Asset From Dashboard");
#endif
            // 親のリストから削除
            _collection.Remove(data);

#if UNITY_EDITOR
            // サブアセット削除をUndoに登録
            Undo.DestroyObjectImmediate(data);

            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _collection.Count) return;
#if UNITY_EDITOR
            // アセットの変更をUndoに登録
            Undo.RecordObject(this, "Delete Asset From Dashboard");
#endif
            // 親のリストから削除
            var data = _collection[index];
            _collection.RemoveAt(index);
#if UNITY_EDITOR
            // サブアセット削除をUndoに登録
            Undo.DestroyObjectImmediate(data);
            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void Delete(SortedSet<int> indexes)
        {
            if (indexes == null || indexes.Count == 0) return;
            HashSet<T> values = new HashSet<T>();
            foreach (var index in indexes)
            {
                if (index < 0 || index >= _collection.Count) continue;
                values.Add(_collection[index]);
            }
#if UNITY_EDITOR
            // アセットの変更をUndoに登録
            Undo.RecordObject(this, "Delete Asset From Dashboard");
#endif
            // 親のリストから削除
            foreach (var data in values)
            {
                _collection.Remove(data);
            }
#if UNITY_EDITOR
            // サブアセット削除をUndoに登録
            foreach (var data in values)
            {
                Undo.DestroyObjectImmediate(data);
            }
            // 保存
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void Move(int fromIndex, int toIndex) // 要素を移動する
        {
            if (fromIndex == toIndex || fromIndex < 0 || toIndex < 0 ||
                fromIndex >= _collection.Count || toIndex > _collection.Count) return;

#if UNITY_EDITOR
            Undo.RecordObject(this, "Move Dashboard Item");
#endif
            var item = _collection[fromIndex];
            _collection.RemoveAt(fromIndex);
            if (toIndex > fromIndex) toIndex--;
            _collection.Insert(toIndex, item);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void Sort(Comparison<T> comparison)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Sort Dashboard");
#endif
            _collection.Sort(comparison);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
}