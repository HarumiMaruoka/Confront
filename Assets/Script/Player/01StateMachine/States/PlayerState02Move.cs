using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState02Move : PlayerState00Base
    {
        public override void Enter()
        {
            _stateMachine.PlayerController.CanMove = true;
        }
        public override void Exit()
        {
            _stateMachine.PlayerController.CanMove = false;
        }
        public override void Update()
        {
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (!_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
            // 攻撃入力が検知されたとき、Attackに遷移する
            if (_stateMachine.PlayerController.Input.IsAttack1InputButtonDown() &&
                _stateMachine.Attack1 != null &&
                !_stateMachine.IsAttackIntervalNow)
            {
                _stateMachine.TransitionTo(_stateMachine.Attack1);
                return;
            }
            if (_stateMachine.PlayerController.Input.IsAttack2InputButtonDown() &&
                _stateMachine.Attack2 != null &&
                !_stateMachine.IsAttackIntervalNow)
            {
                _stateMachine.TransitionTo(_stateMachine.Attack2);
                return;
            }
            if (_stateMachine.PlayerController.Input.IsAttack3InputButtonDown() &&
                _stateMachine.Attack3 != null &&
                !_stateMachine.IsAttackIntervalNow)
            {
                _stateMachine.TransitionTo(_stateMachine.Attack3);
                return;
            }
            // ジャンプ入力が検知されたとき、ステートをJumpに遷移する。
            if (_stateMachine.PlayerController.Input.IsJumpInput &&
                _stateMachine.PlayerController.IsReadyJump)
            {
                _stateMachine.TransitionTo(_stateMachine.Jump);
                return;
            }
            // 移動入力が消失したとき、ステートをIdleに遷移する。
            if (!_stateMachine.PlayerController.Input.IsMoveInput)
            {
                _stateMachine.TransitionTo(_stateMachine.Idle);
                return;
            }
        }
    }
}