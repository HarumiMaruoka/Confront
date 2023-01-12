namespace Player
{
    [System.Serializable]
    public struct EquipmentStatus
    {
        public float _maxHP;
        public float _maxStamina;
        public float _maxMP;
        public float _moveSpeed;
        public float _physicalAttackPower;
        public float _physicalDefensePower;
        public float _magicAttackPower;
        public float _magicDefensePower;

        public static EquipmentStatus operator +(EquipmentStatus a, EquipmentStatus b)
        {
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
    public enum EquipmentType
    {
        Head, Body, Leg, Arm
    }
}