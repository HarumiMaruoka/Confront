using System;
using UnityEngine;

/// <summary>
/// ダメージを与える振る舞いのインターフェース
/// </summary>
public class OnDamageBehavior
{
    public virtual void OnDamage
        (ref float opponentLife, float damage,
        Rigidbody rb = default, Vector3 dir = default, float power = default)
    {
        // 相手のライフを減らす
        opponentLife -= damage;
        // ノックバックする
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }
}