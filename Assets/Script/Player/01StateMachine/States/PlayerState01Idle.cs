using System;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerState01Idle : PlayerState00Base
    {
        public override void Update()
        {
            if (_stateMachine.PlayerController.Input.IsMoveInput)
            {
                _stateMachine.TransitionTo(_stateMachine.StateMove);
            }
        }
    }
}