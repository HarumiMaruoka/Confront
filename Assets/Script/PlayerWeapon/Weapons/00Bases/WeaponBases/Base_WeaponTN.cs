using System;
using UnityEngine;

/// <summary>
/// Weaponの基底クラス。（TNとはTypeNeverの略）<br/>
/// Fireボタン押下中ずっと実行する武器の基底クラス。
/// </summary>
[System.Serializable]
public abstract class Base_WeaponTN : Base_Weapon
{
    // コンストラクタ
    public Base_WeaponTN(GameObject bulletPrefab, PWeaponComponent pWeaponComponent) :
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
            _pWeaponComponent.RightArmAttackMoment = null;
            _pWeaponComponent.RightArmAttackNever = OnFire;
        }
        else if (which == PConstant.LEFT)
        {
            _pWeaponComponent.LeftArmAttackMoment = null;
            _pWeaponComponent.LeftArmAttackNever = OnFire;
        }
        else
        {
            Debug.LogError("<color=grey>不正値が渡されました！修正してください！</color>");
        }
    }
}