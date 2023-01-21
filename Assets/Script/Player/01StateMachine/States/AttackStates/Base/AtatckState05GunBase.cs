using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 銃攻撃ステートのベースクラス
    /// </summary>
    public class AtatckState05GunBase : PlayerState05AttackBase
    {
        public override void Enter()
        {
            // 弾を1つ以上所持している時
            //   銃を構えるアニメーションを再生する。
            //   弾を1つ減らす。
            if (true /* 弾の所持数の判定は省略 */ )
            {
                ChangeAnimation(0);
            }
            // そうでないとき 他のステートに遷移する。
            else
            {
                Transition();
            }
        }
        public override void Update()
        {
            // 銃を構えるアニメーションの再生が完了したとき
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.HoldWeapon))
            {
                ChangeAnimation(1);
            }
            // 銃を撃つアニメーションの再生が完了したとき
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // 弾を1つ以上所持しており、攻撃入力があるとき
                if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButton()
                    /********* 弾の所持数の判定は、未実装の為、省略 *********/)
                {
                    // 弾を1つ減らす。

                    // アニメーションはループ再生で表現するためコード不要
                }
                else
                {
                    ChangeAnimation(2);
                }
            }
            // 銃を収めるアニメーションの再生が完了したとき
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                Transition();
            }
        }
        public override void Exit()
        {
            // このステートの初期化処理を書く
        }
        protected override void Transition()
        {
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (!_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
            // 着地アニメーションの再生が終了したとき遷移処理を実行する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Land))
            {
                // 移動入力があるとき、ステートをMoveに遷移する。
                if (_stateMachine.PlayerController.Input.IsMoveInput)
                {
                    _stateMachine.TransitionTo(_stateMachine.Move);
                    return;
                }
                // 移動入力があるとき、ステートをMoveに遷移する。
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.Idle);
                    return;
                }
            }
        }
    }
}
