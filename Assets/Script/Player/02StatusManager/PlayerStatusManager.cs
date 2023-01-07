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

        private EquipmentManager _equipmentManager = new EquipmentManager();
        private WeaponManager _weaponManager = new WeaponManager();
        private LevelManager _levelManager = new LevelManager();

        public void LoadData()
        {
            // 各データベースに値を設定する。

            // セーブデータを保存する。
        }

        public PlayerStatus BaseStatus => _baseStatus;
        public EquipmentManager EquipmentManager => _equipmentManager;
        public WeaponManager WeaponManager => _weaponManager;
        public LevelManager LevelManager => _levelManager;
        public PlayerStatus TotalStatus =>
            _baseStatus +
            _equipmentManager.EquippedData.TotalStatus +
            _weaponManager.EquippedWeapon.TotalStatus +
            _levelManager.TotalStatus;
    }
}