using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class WeaponDataBase
    {
        public void LoadData(string addressableName)
        {
            // 各武器のパラメータをcsvファイルから読み込む。
            // （もしかしてセルのデータを数値にすればfloat.Parse()する必要ない？）
        }

        private Weapon[] _weaponsData = new Weapon[Constants.MaxWeaponID];
        public Weapon[] WeaponsData => _weaponsData;
    }

    /// <summary>
    /// ひとつの武器を表現するクラス
    /// </summary>
    public class Weapon
    {
        // 武器名
        private string _name = "未設定";
        public string Name => _name;

        // 説明文
        private string _explanatoryText = "未設定";
        public string ExplanatoryText => _explanatoryText;

        // ステータスパラメータ
        private WeaponStatus _status = default;
        public WeaponStatus Status => _status;

        // ステート
        private PlayerState05AttackBase _attackState = default;
        private PlayerState05AttackBase _midairAttackState = default;
        public PlayerState05AttackBase AttackState => _attackState;
        public PlayerState05AttackBase MidairAttackState => _midairAttackState;

        public Weapon(string name, string explanatoryText,
            WeaponStatus status, PlayerState05AttackBase attackState,
            PlayerState05AttackBase midairAttackState)
        {
            _name = name;
            _explanatoryText = explanatoryText;
            _status = status;
            _attackState = attackState;
            _midairAttackState = midairAttackState;
        }
    }
}