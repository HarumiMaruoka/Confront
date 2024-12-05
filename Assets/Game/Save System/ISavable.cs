using System;
using UnityEngine;

namespace Confront.SaveSystem
{
    public interface ISavable
    {
        // SaveDataの参照を渡すので、そこに保存したいデータを書きこんでください。
        void Save(SaveData saveData);
    }
}