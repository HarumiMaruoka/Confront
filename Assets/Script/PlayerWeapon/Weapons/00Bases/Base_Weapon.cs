using System;
using UnityEngine;

[System.Serializable]
public abstract class Base_WeaponBase
{
    /// <summary> 弾のプレハブ </summary>
    GameObject _bulletPrefab = default;
    /// <summary>  </summary>
    PWeaponComponent _pWeaponComponent = default;
    /// <summary>
    /// 攻撃方法（入力がある間ずっと攻撃を実行するか、入力があった瞬間だけ攻撃するか）
    /// </summary>
    FireType _myFireType = FireType.NOT_SET;

    /// <summary> コンストラクタ </summary>
    Base_WeaponBase(GameObject bulletPrefab, PWeaponComponent pWeaponComponent)
    {
        _bulletPrefab = bulletPrefab;
        _pWeaponComponent = pWeaponComponent;

        Init();
    }

    protected virtual void Init() { }

    /// <summary> 攻撃 </summary>
    protected abstract void OnFire(bool whichClick);

    public void SetFire(int whichClick)
    {
        if (whichClick == PConstant.Right_Click)
        {
            if (_myFireType == FireType.MOMENT)
            {

            }
            else if (_myFireType == FireType.NEVER)
            {

            }
            else Debug.LogError("<color=grey>FireTypeを設定してください！</color>");
        }
        else if (whichClick == PConstant.Left_Click)
        {
            if (_myFireType == FireType.MOMENT)
            {

            }
            else if (_myFireType == FireType.NEVER)
            {

            }
            else Debug.LogError("<color=grey>FireTypeを設定してください！</color>");
        }
        else
        {
            Debug.LogError("<color=grey>whichClickが不正値です！</color>");
        }
    }
}

public enum FireType
{
    NOT_SET,
    MOMENT,
    NEVER,
}