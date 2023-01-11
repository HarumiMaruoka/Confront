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
            // UŒ‚“ü—Í‚ªŒŸ’m‚³‚ê‚½‚Æ‚«AMidairAttack‚É‘JˆÚ‚·‚é
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
            // Ú’nó‘Ô‚ªŒŸo‚³‚ê‚½‚Æ‚«ALand‚É‘JˆÚ‚·‚é
            if (_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Land);
                return;
            }
        }
    }
}