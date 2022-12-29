using UnityEngine;

[System.Serializable]
public struct PlayerStatus
{
    [Header("ライフ・スタミナ・マジックポイント")]
    [SerializeField]
    private float _maxHP;
    [SerializeField]
    private float _hp;
    [SerializeField]
    private float _maxStamina;
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private float _maxMagicPoint;
    [SerializeField]
    private float _magicPoint;

    [Header("移動速度")]
    [SerializeField]
    private float _moveSpeed;

    [Header("プレイヤー攻撃力（武器攻撃力と併せて使用する）")]
    [SerializeField]
    private float _musclePower;
    [SerializeField]
    private float _magicPower;
    [Header("防御力")]
    [SerializeField]
    private float _muscleDefense;
    [SerializeField]
    private float _magicDefense;

    [Header("その他")]
    [SerializeField]
    private int _level;
    [SerializeField]
    private float _exp;
    [SerializeField]
    private float _money;
}