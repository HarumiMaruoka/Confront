using System;
using UnityEngine;

/// <summary>
/// 武器の基底クラス。<>
/// </summary>
[System.Serializable]
public abstract class Base_Weapon
{
    /// <summary> 弾のプレハブ </summary>
    private readonly GameObject _bulletPrefab = default;
    /// <summary>  </summary>
    protected readonly PWeaponComponent _pWeaponComponent = default;

    /// <summary> コンストラクタ </summary>
    public Base_Weapon(GameObject bulletPrefab, PWeaponComponent pWeaponComponent)
    {
        _bulletPrefab = bulletPrefab;
        _pWeaponComponent = pWeaponComponent;

        Init();
    }

    protected virtual void Init() { }

    /// <summary> 攻撃処理 </summary>
    protected abstract void OnFire();
}