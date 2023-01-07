using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 装備中の装備を管理する、所持している装備を管理するクラス。
    /// 仕様が不安定なので仕様書にまとめてから実装する。
    /// </summary>
    [System.Serializable]
    public class EquipmentManager
    {
        /// <summary> 着用している装備データ </summary>
        public EquippedData EquippedData { get; private set; } = new EquippedData();
        /// <summary> 装備のデータベース </summary>
        public EquipmentDataBase DataBase { get; private set; } = new EquipmentDataBase();
    }
}