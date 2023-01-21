using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 魔法攻撃のステートのベースクラス
    /// </summary>
    public class AtatckState05MagicBase : PlayerState05AttackBase
    {
        public override void Enter()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

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

        // 魔法攻撃の際にあったら便利そうなメソッド群を記述する。

    }
}
