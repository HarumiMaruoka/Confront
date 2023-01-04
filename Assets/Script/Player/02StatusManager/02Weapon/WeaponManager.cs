using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 装備中の武器を管理する、所持している武器を管理するクラス。
    /// 仕様が不安定なので仕様書にまとめてから実装する。
    /// </summary>
    [System.Serializable]
    public class WeaponManager
    {
        private WeaponDataBase _weaponDataBase = new WeaponDataBase();
        /// <summary> 全てのWeaponデータを管理･格納しているオブジェクトへの参照 </summary>
        public WeaponDataBase WeaponDataBase => _weaponDataBase;

        private EquippedWeapon _equippedWeapon = new EquippedWeapon();
        /// <summary> 装備中の武器を管理しているオブジェクトへの参照 </summary>
        public EquippedWeapon EquippedWeapon => _equippedWeapon;

    }
}