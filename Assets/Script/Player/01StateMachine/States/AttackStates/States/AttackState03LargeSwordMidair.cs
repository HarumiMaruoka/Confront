using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class AttackState03LargeSwordMidair : PlayerState05AttackBase
    {
        public override void Enter()
        {
            // ‚’¼‚ÌˆÚ“®ŒvZ‚ğ’â~‚·‚é
            _stateMachine.PlayerController.IsVerticalCalculation = false;
        }
        public override void Exit()
        {
            // ‚’¼‚ÌˆÚ“®ŒvZ‚ğŠJn‚·‚é
            _stateMachine.PlayerController.IsVerticalCalculation = true;
        }
    }
}