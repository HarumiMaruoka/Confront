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
        public Equipment Head => _head;
        // 胴
        private Equipment _body = null;
        public Equipment Body => _body;
        // 足
        private Equipment _leg = null;
        public Equipment Leg => _leg;
        // 左腕
        private Equipment _leftHand = null;
        public Equipment LeftHand => _leftHand;
        // 右腕
        private Equipment _rightHand = null;
        public Equipment RightHand => _rightHand;

        public void LoadData()
        {

        }
        public void CahageEquipped(Equipment equipment)
        {

        }
    }
}