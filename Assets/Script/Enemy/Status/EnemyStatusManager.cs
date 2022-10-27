using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のステータスを管理するコンポーネント。
/// </summary>
public class EnemyStatusManager : MonoBehaviour
{
    [Header("エネミーに設定すべき値")]
    [Tooltip(""), SerializeField]
    private EnemyStatus _status = default;

    public EnemyStatus Status { get => _status; set => _status = value; }
}