using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class AttackState02NomalSword : PlayerState05AttackBase
    {
        public override void Update()
        {
            // アニメーション終了時, Attackアニメーションの完了が検知されたときステートを遷移する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // ここに遷移処理を記述する
            }
            // アニメーション再生中, 入力が発生したとき次のコンボを再生する

        }
        private void WaitNextComboInput(int nextNumber)
        {

        }
    }
}