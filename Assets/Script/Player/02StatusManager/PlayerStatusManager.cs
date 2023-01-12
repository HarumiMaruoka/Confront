using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// プレイヤーのステータスを制御するクラス
    /// </summary>
    [System.Serializable]
    public class PlayerStatusManager
    {
        [SerializeField, Tooltip("基本となるステータス")]
        private PlayerStatus _baseStatus = default;

        [SerializeField]
        private EquipmentManager _equipmentManager = default;
        [SerializeField]
        private WeaponManager _weaponManager = default;
        [SerializeField]
        private LevelManager _levelManager = default;

        public void LoadData()
        {
            // 各データベースに値を設定する。

            // セーブデータを取得し、フィールドに保存する。

        }

        public PlayerStatus BaseStatus => _baseStatus;
        public EquipmentManager EquipmentManager => _equipmentManager;
        public WeaponManager WeaponManager => _weaponManager;
        public LevelManager LevelManager => _levelManager;
        public PlayerStatus TotalStatus =>
            _baseStatus +
            _equipmentManager.EquippedData.TotalStatus +
            _weaponManager.EquippedWeapon.TotalStatus +
            _levelManager.CurrentStatus;
    }
}