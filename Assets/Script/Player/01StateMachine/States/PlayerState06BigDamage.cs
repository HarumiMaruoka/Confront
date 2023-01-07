using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState06BigDamage : PlayerState00Base
    {
        // BigDamage仕様書がドライブのスプレッドシートに用意してあるので
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

        [AnimationParameter, SerializeField]
        private string _standUpAnimParameterName = default;
        public override void Update()
        {
            // 吹っ飛びが停止したとき立ち上がるアニメーションを再生する。
            // if (_stateMachine.PlayerController.Rigidbody.velocity.sqrMagnitude < 0.1f)
            {
                // _animator.SetBool(_standUpAnimParameterName,true);
                return;
            }
            // 立ち上がるアニメーションの再生が終了したとき遷移処理を実行する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.StandUp))
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