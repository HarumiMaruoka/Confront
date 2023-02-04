using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState08Land : PlayerState00Base
    {
        [Tooltip("着地時もちょっとだけ動けるようにする。"), SerializeField, Range(0f, 1f)]
        private float _moveAcceleration = default;
        // 移動加速度を元に戻すように保存するフィールド
        private float _saveAccelerationValue = default;

        public override void Enter()
        {
            _stateMachine.PlayerController.CanMove = true;
            _saveAccelerationValue = _stateMachine.PlayerController.MoveHorizontalAcceleration;
            _stateMachine.PlayerController.MoveHorizontalAcceleration = _moveAcceleration;
            _stateMachine.PlayerController.ResetMoveHorizontalSpeed();
        }
        public override void Exit()
        {
            _stateMachine.PlayerController.CanMove = false;
            _stateMachine.PlayerController.MoveHorizontalAcceleration = _saveAccelerationValue;
            _stateMachine.PlayerController.ResetMoveHorizontalSpeed();
        }
        public override void Update()
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