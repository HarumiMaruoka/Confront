using System;
using UnityEngine;


namespace Player
{
    // 攻撃の仕様 : 
    // 武器の数だけ攻撃ステート（Land版とMidair版）を作成する。
    // 武器切り替え時に実行するステートを変更する。

    /// <summary>
    /// 全ての攻撃ステートの基底クラス
    /// </summary>
    [System.Serializable]
    public class PlayerState05AttackBase : PlayerState00Base
    {
        // 全ての攻撃ステートに共通して必要な値を記述してください。

        // 攻撃入力の受付を開始する

        // 攻撃入力の受付を停止する 
    }
}