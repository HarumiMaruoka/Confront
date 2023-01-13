using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class AttackState00BasicGunFire : PlayerState05AttackBase
    {
        public override void Update()
        {
            // 構えアニメーションが完了したらFireアニメーションを再生する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.HoldWeapon))
            {

            }
            // Fireアニメーションの完了時に
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // 攻撃入力があればもう一度再生する。
                if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                 _stateMachine.PlayerController.Input.IsAttack2InputButton())
                {

                }
                // そうでなければ, 武器を構えを解くアニメーションを再生する。
                else
                {

                }
            }
            // 武器の構えを解くアニメーションの再生が完了したら, 他のステートに遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {

            }
        }
    }
}