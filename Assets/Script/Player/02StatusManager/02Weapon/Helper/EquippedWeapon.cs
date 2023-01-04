using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 装備中の武器を管理するクラス
    /// </summary>
    [System.Serializable]
    public class EquippedWeapon
    {
        public void LoadData()
        {

        }

        // 装備している武器
        private Weapon _leftHandWeapon = null;
        private Weapon _rightHandWeapon = null;
        public Weapon LeftHandWeapon { get => _leftHandWeapon; set => _leftHandWeapon = value; }
        public Weapon RightHandWeapon { get => _rightHandWeapon; set => _rightHandWeapon = value; }

        /// <summary> 装備している武器のステータスの合計値 </summary>
        public WeaponStatus TotalStatus => _leftHandWeapon.Status + _rightHandWeapon.Status;
    }
}