using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState06MiddleDamage : PlayerState00Base
    {
        public override void Exit()
        {
            // ゴッドモードを停止する。(ゴッドモードの開始処理は, ダメージ時に実行する)
            _stateMachine.PlayerController.EndGodMode();
            _stateMachine.PlayerController.
                ChangeMovementMethod(PlayerController.MovementMethodType.CharacterController);
        }

        public override void Update()
        {
            // スモールダメージ用アニメーションの再生が完了したとき状態を遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.SmallDamage))
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
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