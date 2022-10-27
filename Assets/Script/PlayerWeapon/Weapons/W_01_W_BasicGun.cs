using System;
using UnityEngine;

/// <summary>
/// 基本的な銃のクラス
/// </summary>
[System.Serializable]
public class W_01_W_BasicGun : Base_WeaponTMUseRay
{
    public W_01_W_BasicGun(GameObject bulletPrefab, PWeaponComponent pWeaponComponent, float maxDistance) :
        base(bulletPrefab, pWeaponComponent, maxDistance)
    {

    }

    /// <summary>
    /// 前方にレイを飛ばし攻撃対象にヒットした場合、
    /// 対象に攻撃処理を加える。
    /// </summary>
    protected override void OnFire()
    {
        if (OnFireRay(_maxDistance))
        {
            if(_hitInfo.collider.TryGetComponent(out EnemyStatusManager eStatusManager))
            {
                OnDamage(eStatusManager, PStatusManager.Instance.TotalStatus._longRangeOffensivePower);
            }
        }
    }

    protected override bool OnFireRay(float maxDistance)
    {
        // 武器から正面に向かってまっすぐレイを飛ばす。
        return Physics.Raycast(
            _pWeaponComponent.transform.position, _pWeaponComponent.transform.forward,
            out _hitInfo, maxDistance);
    }
}