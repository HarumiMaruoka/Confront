using UnityEngine;

namespace Player
{
    [System.Serializable]
    public struct PlayerStatus
    {
        [Header("ライフ・スタミナ・マジックポイント")]
        [SerializeField]
        public float _maxHP;
        [SerializeField]
        public float _hp;
        [SerializeField]
        public float _maxStamina;
        [SerializeField]
        public float _stamina;
        [SerializeField]
        public float _maxMagicPoint;
        [SerializeField]
        public float _magicPoint;

        [Header("移動速度")]
        [SerializeField]
        public float _moveSpeed;

        [Header("プレイヤー攻撃力（武器攻撃力と併せて使用する）")]
        [SerializeField]
        public float _musclePower;
        [SerializeField]
        public float _magicPower;
        [Header("防御力")]
        [SerializeField]
        public float _muscleDefense;
        [SerializeField]
        public float _magicDefense;

        [Header("レベル, 経験値, お金")]
        [SerializeField]
        public int _level;
        [SerializeField]
        public float _exp;
        [SerializeField]
        public float _money;

        /// <summary>
        /// 装備ステータスの + 演算子適用
        /// </summary>
        public static PlayerStatus operator +(PlayerStatus a, PlayerStatus b)
        {
            a._maxHP += b._maxHP;
            a._maxStamina += b._maxStamina;
            a._maxMagicPoint += b._maxMagicPoint;
            a._moveSpeed += b._moveSpeed;
            a._musclePower += b._musclePower;
            a._muscleDefense += b._muscleDefense;
            a._magicPower += b._magicPower;
            a._magicDefense += b._magicDefense;

            return a;
        }
        /// <summary>
        /// 装備ステータスの + 演算子適用
        /// </summary>
        public static PlayerStatus operator +(PlayerStatus a, EquipmentStatus b)
        {
            // 最大体力, スタミナ, マジックポイント, 移動速度の適用
            a._maxHP += b._maxHP;
            a._maxStamina += b._maxStamina;
            a._maxMagicPoint += b._maxMP;
            a._moveSpeed += b._moveSpeed;
            // 各攻撃力, 防御力の適用
            a._musclePower += b._physicalAttackPower;
            a._muscleDefense += b._physicalDefensePower;
            a._magicPower += b._magicAttackPower;
            a._magicDefense += b._magicDefensePower;

            return a;
        }
        /// <summary>
        /// レベルステータスの + 演算子適用
        /// </summary>
        public static PlayerStatus operator +(PlayerStatus a, LevelStatus b)
        {
            // 最大体力, スタミナ, マジックポイント, 移動速度の適用
            a._maxHP += b._maxHP;
            a._maxStamina += b._maxStamina;
            a._maxMagicPoint += b._maxMP;
            a._moveSpeed += b._moveSpeed;
            // 各攻撃力, 防御力の適用
            a._musclePower += b._physicalAttackPower;
            a._muscleDefense += b._physicalDefensePower;
            a._magicPower += b._magicAttackPower;
            a._magicDefense += b._magicDefensePower;

            return a;
        }
        /// <summary>
        /// 武器ステータスの + 演算子適用
        /// </summary>
        public static PlayerStatus operator +(PlayerStatus a, WeaponStatus b)
        {
            // 最大体力, スタミナ, マジックポイント, 移動速度の適用
            a._maxHP += b._maxHP;
            a._maxStamina += b._maxStamina;
            a._maxMagicPoint += b._maxMP;
            a._moveSpeed += b._moveSpeed;
            // 武器の攻撃値, 防御値はダメージ計算の際に別で計算する為 適用しない。

            return a;
        }
    }
}