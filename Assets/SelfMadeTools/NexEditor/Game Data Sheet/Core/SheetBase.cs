using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NexEditor.GameDataSheet
{
    public class SheetBase<T> : ScriptableObject, IEnumerable<T> where T : ScriptableObject
    {
        [SerializeField]
        private List<T> _collection;
        public IReadOnlyList<T> Collection => _collection ??= new List<T>();

        public T Create()
        {
            var instance = CreateInstance<T>();

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(instance, "Create Character");
            Undo.RecordObject(this, "Create Character");
#endif

            _collection.Add(instance);
            instance.name = ToString();

#if UNITY_EDITOR
            AssetDatabase.AddObjectToAsset(instance, this);
            Undo.RecordObject(instance, "Create Character");
            Undo.RegisterCreatedObjectUndo(instance, "Create Character");

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
#endif

            return instance;
        }

        public void Delete(T data)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Delete Character");
#endif

            _collection.Remove(data);

#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(data);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T this[int i] => _collection[i];

        public int Count => _collection.Count;
    }
}