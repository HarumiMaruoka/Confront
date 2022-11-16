using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステータスの振る舞いをするクラス。
/// </summary>
public class EnemyStatusBehavior
{
    public EnemyStatusBehavior(EnemyStatus status)
    {
        Status = status;
        Life = status._life;
    }

    public readonly EnemyStatus Status = default;

    public float Life { get; set; }
}