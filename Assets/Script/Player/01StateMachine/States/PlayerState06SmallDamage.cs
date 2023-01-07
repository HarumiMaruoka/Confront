using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState06SmallDamage : PlayerState00Base
    {
        // SmallDamage仕様書がドライブのスプレッドシートに用意してあるので
        // それに沿って記述してください。

        public override void Enter()
        {
            // ゴッドモードを起動する
            // _stateMachine.PlayerController.StartGodMode();
            // 物理演算を有効化する
            // _stateMachine.PlayerController.Rigidbody.isKinematic = false;
            // ノックバックする。
            // ここにノックバックのコードを記述する。
        }
        public override void Exit()
        {
            // ゴッドモードを停止する
            // _stateMachine.PlayerController.EndGodMode();
            // 基本的にはキャラクターコントローラーで移動等の制御をするので、物理演算を無効化する。
            // _stateMachine.PlayerController.Rigidbody.isKinematic = false;
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