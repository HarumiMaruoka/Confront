using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerState01Idle : PlayerState00Base
    {
        public override void Update()
        {
            // 移動入力が検知されたとき、ステートをMoveに遷移する。
            if (_stateMachine.PlayerController.Input.IsMoveInput)
            {
                _stateMachine.TransitionTo(_stateMachine.Move);
                return;
            }
            // ジャンプ入力が検知されたとき、ステートをJumpに遷移する。
            if (_stateMachine.PlayerController.Input.IsJumpInput)
            {
                _stateMachine.TransitionTo(_stateMachine.Jump);
                return;
            }
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (_stateMachine.PlayerController.GroundChecker.IsExist())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
        }
    }
}