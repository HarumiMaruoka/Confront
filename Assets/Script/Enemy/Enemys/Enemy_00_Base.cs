using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーに関する更新をまとめて実行するコンポーネント
/// </summary>
public abstract class Enemy_00_Base : MonoBehaviour
{
    /// <summary> 毎フレーム実行する処理をこの変数に登録してください </summary>
    public System.Action Processor { get => _processor; set => _processor = value; }
    protected System.Action _processor = default;

    private void Update()
    {
        _processor();
    }

    /// <summary>
    /// このメソッド内で振る舞いを登録してください
    /// </summary>
    protected abstract void Register();
}