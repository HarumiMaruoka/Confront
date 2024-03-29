using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState04Midair : PlayerState00Base
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
            // 接地状態が検出されたとき、Landに遷移する
            if (_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Land);
                return;
            }
        }
    }
}