using System;
using UnityEngine;

/// <summary>
/// 正面に攻撃（レイ）を放つ振る舞いクラス <br/>
/// プレイヤー用
/// </summary>
[System.Serializable]
public class FireRaycastFrontForPlayer : IFireBehavior
{
    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="offensivePower"> ダメージ量 </param>
    /// <param name="originPos"> 撃つ位置 </param>
    /// <param name="dir"> 方向 </param>
    /// <param name="maxDistance"> 最大距離 </param>
    public void OnFire(float offensivePower, Vector3 originPos, Vector3 dir,
    float maxDistance)
    {
        // 敵に接触したらダメージを加える。
        RaycastHit hitInfo = default;
        if(Physics.Raycast(originPos, dir, out hitInfo, maxDistance))
        {
            if(hitInfo.collider.TryGetComponent(out EnemyStatusBehavior eStatusManager))
            {
                eStatusManager.Life -= offensivePower;
            }
        }
    }
}