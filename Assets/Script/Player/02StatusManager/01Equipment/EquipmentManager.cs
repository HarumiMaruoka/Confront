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
        private EquipmentStatus _totalStatus;
        /// <summary> 着用している全ての装備のステータスの合計値 </summary>
        public EquipmentStatus TotalStatus => _totalStatus;
    }
}