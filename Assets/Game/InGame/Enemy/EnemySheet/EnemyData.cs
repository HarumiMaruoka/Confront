using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class EnemyData : ScriptableObject
    {
        public int ID;
        public string Name;
        public string Description;
        public GameObject Prefab;
        public TextAsset DefaultDropItemTable;

        private DropItemData[] _dropItemTable = null;
        public DropItemData[] DropItemTable => _dropItemTable ??= DefaultDropItemTable.LoadDropItemTable();
    }

    public struct DropItemData
    {
        public DropItem.ItemType Type;
        public int ID;
        public int DropRate;
    }

    public static class TextAssetExtensions
    {
        public static DropItemData[] LoadDropItemTable(this TextAsset textAsset, int ignoreRows = 1, int ignoreColumn = 0)
        {
            var lines = textAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var dropItemTable = new DropItemData[lines.Length - ignoreRows];
            for (int i = ignoreRows; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',');
                dropItemTable[i - ignoreRows] = new DropItemData
                {
                    Type = (DropItem.ItemType)Enum.Parse(typeof(DropItem.ItemType), columns[ignoreColumn]),
                    ID = int.Parse(columns[ignoreColumn + 1]),
                    DropRate = int.Parse(columns[ignoreColumn + 2]),
                };
            }
            return dropItemTable;
        }
    }
}