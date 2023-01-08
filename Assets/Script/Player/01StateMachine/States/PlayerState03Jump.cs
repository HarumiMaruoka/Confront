using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState03Jump : PlayerState00Base
    {
        public override void Update()
        {
            // アニメーションの再生が終了したとき遷移処理を実行する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Jump))
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
                // 移動入力があるとき、ステートをMoveに遷移する。
                if (_stateMachine.PlayerController.Input.IsMoveInput)
                {
                    _stateMachine.TransitionTo(_stateMachine.Move);
                    return;
                }
                // 移動入力があるとき、ステートをMoveに遷移する。
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.Idle);
                    return;
                }
            }
            _stateMachine.PlayerController.Move();
        }
    }
}