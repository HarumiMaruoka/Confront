using System;
using UniRx;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState06BigDamage : PlayerState00Base
    {
        public override void Exit()
        {
            // ゴッドモードを停止する
            _stateMachine.PlayerController.EndGodMode();
            _stateMachine.PlayerController.
                ChangeMovementMethod(PlayerController.MovementMethodType.CharacterController);
        }

        [AnimationParameter, SerializeField]
        private string _standUpAnimParameterName = default;
        public override void Update()
        {
            // 速度がほぼ停止したとき立ち上がるアニメーションを再生する。
            if (_stateMachine.PlayerController.Rigidbody.velocity.sqrMagnitude < 0.1f)
            {
                _stateMachine.PlayerController.Animator.SetBool(_enterAnimParameterName, false);
                _stateMachine.PlayerController.Animator.SetBool(_standUpAnimParameterName, true);
                return;
            }
            // 立ち上がるアニメーションの再生が終了したとき遷移処理を実行する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.StandUp))
            {
                _stateMachine.PlayerController.Animator.SetBool(_standUpAnimParameterName, false);
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