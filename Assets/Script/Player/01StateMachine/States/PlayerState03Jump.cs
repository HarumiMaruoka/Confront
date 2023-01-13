using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState03Jump : PlayerState00Base
    {
        public override void Enter()
        {
            _stateMachine.PlayerController.CamMove = true;
        }
        public override void Exit()
        {
            _stateMachine.PlayerController.CamMove = false;
        }
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
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.Land);
                    return;
                }
            }
        }
    }
}