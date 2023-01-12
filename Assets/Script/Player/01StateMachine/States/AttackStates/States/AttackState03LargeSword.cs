using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class AttackState03LargeSword : PlayerState05AttackBase
    {
        [AnimationParameter, SerializeField]
        private string _secondComboAnimationParameterName;
        [AnimationParameter, SerializeField]
        private string _thirdComboAnimationParameterName;

        public override void Update()
        {

        }
        private void WaitNextComboInput(int nextNumber)
        {

        }
    }
}