using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class AttackState01TwinSword : PlayerState05AttackBase
    {
        public override void Enter()
        {
            _stateMachine.PlayerController.IsVerticalCalculation = false;
        }
        public override void Exit()
        {
            _stateMachine.PlayerController.IsVerticalCalculation = true;
        }

        [Tooltip("二番目以降のコンボアニメーションパラメータ名を設定してください")]
        [AnimationParameter, SerializeField]
        private string[] _comboAnimationParameterName;
        public override void Update()
        {

        }
        private void WaitNextComboInput(int nextNumber)
        {

        }
    }
}