using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 着用中の装備を管理するクラス
    /// </summary>
    [System.Serializable]
    public class EquippedData
    {
        /// <summary> 着用中の装備ステータスの合計値 </summary>
        public EquipmentStatus TotalStatus =>
            _head.Status + _body.Status + _leg.Status + _leftHand.Status + _rightHand.Status;

        // 頭
        private Equipment _head = null;
        // 胴
        private Equipment _body = null;
        // 足
        private Equipment _leg = null;
        // 左腕
        private Equipment _leftHand = null;
        // 右腕
        private Equipment _rightHand = null;

        public void LoadData()
        {

        }
        public void CahageEquipped()
        {

        }
    }
}