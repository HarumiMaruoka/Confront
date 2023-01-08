namespace Player
{
    [System.Serializable]
    public struct LevelStatus
    {
        public float _maxHP;
        public float _maxStamina;
        public float _maxMP;
        public float _moveSpeed;
        public float _physicalAttackPower;
        public float _physicalDefensePower;
        public float _magicAttackPower;
        public float _magicDefensePower;

        public static LevelStatus operator +(LevelStatus a, LevelStatus b)
        {
            // 最大体力, スタミナ, マジックポイント, 移動速度の適用
            a._maxHP += b._maxHP;
            a._maxStamina += b._maxStamina;
            a._maxMP += b._maxMP;
            a._moveSpeed += b._moveSpeed;
            a._physicalAttackPower += b._physicalAttackPower;
            a._physicalDefensePower += b._physicalDefensePower;
            a._magicAttackPower += b._magicAttackPower;
            a._magicDefensePower += b._magicDefensePower;

            return a;
        }
    }
}