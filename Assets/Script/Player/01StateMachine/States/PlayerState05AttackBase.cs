using System;
using UnityEngine;


namespace Player
{
    // 攻撃の仕様
    // 武器の数だけ攻撃ステートを作成。
    // 武器切り替え時にステートも変更する。

    [System.Serializable]
    public class PlayerState05AttackBase : PlayerState00Base
    {
        [AnimationParameter, SerializeField]
        private string _animationParameterName = default;

    }
}