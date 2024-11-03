using System;
using UnityEngine;

namespace Confront.Player
{
    public interface IPlayerState
    {
        string AnimationStateName { get; } // アニメーションステート名
        bool IsMovementInputEnabled { get; } // 移動入力が有効かどうか
        void Enter(PlayerController player);
        void Update(PlayerController player);
        void Exit(PlayerController player);
    }
}