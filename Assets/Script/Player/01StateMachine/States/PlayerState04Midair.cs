using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState04Midair : PlayerState00Base
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
            // 攻撃入力が検知されたとき、MidairAttackに遷移する
            if (_stateMachine.PlayerController.Input.IsAttack1InputButton &&
                _stateMachine.MidairAttack1 != null)
            {
                _stateMachine.TransitionTo(_stateMachine.MidairAttack1);
                return;
            }
            if (_stateMachine.PlayerController.Input.IsAttack2InputButton &&
                _stateMachine.MidairAttack2 != null)
            {
                _stateMachine.TransitionTo(_stateMachine.MidairAttack2);
                return;
            }
            // 接地状態が検出されたとき、Landに遷移する
            if (_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Land);
                return;
            }
        }
    }
}