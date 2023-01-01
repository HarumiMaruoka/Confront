using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class WeaponDataBase
    {
        // 各武器のパラメータをcsvファイルから読み込む。
        // （もしかしてセルのデータを数値にすればfloat.Parse()する必要ない？）
        public void LoadData()
        {

        }
        public const int MaxID = 0;

        private Weapon[] _weaponsData = new Weapon[MaxID];
        public Weapon[] WeaponsData => _weaponsData;
    }

    /// <summary>
    /// ひとつの武器を表現するクラス
    /// </summary>
    public class Weapon
    {
        // 武器名
        private string _name = default;
        public string Name => _name;

        // 説明文
        private string _explanatoryText = default;
        public string ExplanatoryText => _explanatoryText;

        // ステータスパラメータ
        private WeaponStatus _status = default;
        public WeaponStatus Status => _status;

        // ステート
        private PlayerState05AttackBase _attackState = default;
        public PlayerState05AttackBase AttackState => _attackState;

        public Weapon(string name, string explanatoryText,
            WeaponStatus status, PlayerState05AttackBase attackState)
        {
            _name = name;
            _explanatoryText = explanatoryText;
            _status = status;
            _attackState = attackState;
        }
    }
}