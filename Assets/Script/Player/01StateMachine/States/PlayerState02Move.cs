using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState02Move : PlayerState00Base
    {
        public override void Update()
        {
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (!_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
            // 会話入力が検出されたとき かつ プレイヤーの前方にHumanが検出されたとき、Talkに遷移する
            if (_stateMachine.PlayerController.Input.IsTalkInput &&
                _stateMachine.PlayerController.TalkChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Talk);
                return;
            }
            // 攻撃入力が検知されたとき、Attackに遷移する
            if (_stateMachine.PlayerController.Input.IsAttack1InputButton &&
                _stateMachine.Attack1 != null)
            {
                _stateMachine.TransitionTo(_stateMachine.Attack1);
                return;
            }
            if (_stateMachine.PlayerController.Input.IsAttack2InputButton &&
                _stateMachine.Attack2 != null)
            {
                _stateMachine.TransitionTo(_stateMachine.Attack2);
                return;
            }
            // ジャンプ入力が検知されたとき、ステートをJumpに遷移する。
            if (_stateMachine.PlayerController.Input.IsJumpInput)
            {
                _stateMachine.TransitionTo(_stateMachine.Jump);
                return;
            }
            // 移動入力が消失したとき、ステートをIdleに遷移する。
            if (!_stateMachine.PlayerController.Input.IsMoveInput)
            {
                _stateMachine.TransitionTo(_stateMachine.Move);
                return;
            }
        }
    }
}