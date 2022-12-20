using System;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    [Header("ライフ・スタミナ・マジックポイント")]
    [SerializeField]
    private float _maxHP = 100f;
    [SerializeField]
    private float _hp = 100f;
    [SerializeField]
    private float _maxStamina = 100f;
    [SerializeField]
    private float _stamina = 100f;
    [SerializeField]
    private float _maxMagicPoint = 100f;
    [SerializeField]
    private float _magicPoint = 100f;

    [Header("移動速度")]
    [SerializeField]
    private float _moveSpeed = 1f;

    [Header("プレイヤー攻撃力（武器攻撃力と併せて使用する）")]
    [SerializeField]
    private float _musclePower = 100f;
    [SerializeField]
    private float _magicPower = 100f;

    [Header("その他")]
    [SerializeField]
    private int _level = 0;
    [SerializeField]
    private float _exp = 0f;
    [SerializeField]
    private float _money = 0f;


    public float MaxHP => _maxHP;
    public float HP => _hp;
    public float MaxStamina => _maxStamina;
    public float Stamina => _stamina;
    public float MaxMagicPoint => _maxMagicPoint;
    public float MagicPoint => _magicPoint;
    public float MoveSpeed => _moveSpeed;
    public float MusclePower => _musclePower;
    public float MagicPower => _magicPower;
    public int Level => _level;
    public float Exp => _exp;
    public float Money => _money;
}