using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class EquipmentDataBase
    {
        public void LoadData(string addressableName)
        {
            // 各装備のパラメータをcsvファイルから読み込む。
            // （もしかしてセルのデータを数値にすればfloat.Parse()する必要ない？）
        }

        private Equipment[] _equipmentData = new Equipment[Constants.MaxEquipmentID];

        public Equipment[] EquipmentData => _equipmentData;
    }

    public class Equipment
    {
        // ステータス変化パラメータ
        private EquipmentStatus _status = default;
        public EquipmentStatus Status => _status;
        // 装備の名前
        private string _name = "未設定";
        public string Name => _name;
        // 説明文
        private string _explanatoryText = "未設定";
        public string ExplanatoryText => _explanatoryText;

        public Equipment(EquipmentStatus status, string name, string explanatoryText)
        {
            _status = status;
            _name = name;
            _explanatoryText = explanatoryText;
        }
    }

    [System.Serializable]
    public enum EquippedType
    {
        Head, Body, Leg, LeftHand, RightHand,
    }
}