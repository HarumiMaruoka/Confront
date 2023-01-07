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
        /// <summary> 全てのWeaponデータを管理･格納しているオブジェクトへの参照 </summary>
        public WeaponDataBase WeaponDataBase { get; private set; } = new WeaponDataBase();

        /// <summary> 装備中の武器を管理しているオブジェクトへの参照 </summary>
        public EquippedWeapon EquippedWeapon { get; private set; } = new EquippedWeapon();

    }
}