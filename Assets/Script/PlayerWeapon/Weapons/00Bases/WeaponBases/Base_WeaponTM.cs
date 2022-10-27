using System;
using UnityEngine;

/// <summary>
/// Weaponの基底クラス。（TMとはTypeMomentの略）<br/>
/// Fireボタン押下時のみ実行する武器の基底クラス。
/// </summary>
[System.Serializable]
public abstract class Base_WeaponTM : Base_Weapon
{
    // コンストラクタ
    public Base_WeaponTM(GameObject bulletPrefab, PWeaponComponent pWeaponComponent) :
        base(bulletPrefab, pWeaponComponent)
    {

    }
    /// <summary>
    /// 攻撃処理をコンポーネントにセットする。
    /// </summary>
    /// <param name="which">
    /// 右腕にセットするか、左腕にセットするか。
    /// </param>
    public void SetWeapon(int which)
    {
        if (which == PConstant.RIGHT)
        {
            _pWeaponComponent.RightArmAttackMoment = OnFire;
            _pWeaponComponent.RightArmAttackNever = null;
        }
        else if (which == PConstant.LEFT)
        {
            _pWeaponComponent.LeftArmAttackMoment = OnFire;
            _pWeaponComponent.LeftArmAttackNever = null;
        }
        else
        {
            Debug.LogError("<color=grey>不正値が渡されました！修正してください！</color>");
        }
    }
}