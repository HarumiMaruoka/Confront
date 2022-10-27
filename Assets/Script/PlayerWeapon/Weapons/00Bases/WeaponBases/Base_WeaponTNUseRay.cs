using System;
using UnityEngine;

/// <summary>
/// レイを飛ばすウエポンの基底クラス <br/>
/// 押下中ずっと実行するタイプ
/// </summary>
[System.Serializable]
public abstract class Base_WeaponTNUseRay : Base_WeaponTN, IOnEnemyDamage
{
    protected RaycastHit _hitInfo = default;
    protected readonly float _maxDistance = default;

    // コンストラクタ
    public Base_WeaponTNUseRay(GameObject bulletPrefab, PWeaponComponent pWeaponComponent,float maxDistance) :
        base(bulletPrefab, pWeaponComponent)
    {
        _maxDistance = maxDistance;
    }
    public virtual void OnDamage(
        EnemyStatusManager enemyStatusManager, float value, bool isKnockback = false,
        Rigidbody _rigidbody = null, Vector3 dir = default, float knockbackPower = 1)
    {
        // 体力を減らす処理
        var temp = enemyStatusManager.Status;
        temp._life -= value;
        enemyStatusManager.Status = temp;

        // ノックバック処理
        if (isKnockback)
        {
            if (_rigidbody == null)
            {
                Debug.LogError(
                    $"<color=grey>_rigidbodyがnullです！修正してください！</color>");
            }
            else
            {
                _rigidbody.AddForce(dir.normalized * knockbackPower, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// レイを飛ばす処理。
    /// </summary>
    protected abstract bool OnFireRay(float maxDistance);
}