using Confront.DebugTools;
using Confront.Input;
using Confront.Player.Combo;
using Confront.Utility;
using System;
using UnityEngine;


namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "SwiftAttackState", menuName = "Confront/Player/Combo/SwiftAttackState")]
    public class SwiftAttackState : AttackBehaviour
    {
        [Header("アニメーション")]
        [SerializeField]
        private string _animationName = "None";
        [SerializeField]
        private AnimationCurve _xAxisMovementCurve;
        [SerializeField]
        private AnimationCurve _yAxisMovementCurve;

        [Header("攻撃力")]
        [SerializeField]
        private float _baseAttackPower = 1; // 基本攻撃力
        [SerializeField]
        private float _attackPowerFactor = 1; // 攻撃力係数

        [Header("コンボ入力")]
        [SerializeField]
        private float _comboInputAcceptanceTime = 0.2f; // コンボ入力を受け付ける時間
        [SerializeField]
        private float _comboInputDeactivationTime = 0.2f; // コンボ入力を無効にする時間

        [Header("状態遷移")]
        [SerializeField]
        private float _nextAttackTransitionTime = 0.2f; // 次の攻撃に遷移する時間
        [SerializeField]
        private float _attackCompletionTime = 0.3f; // 攻撃を完了する時間

        [Header("当たり判定_ヒットボックス")]
        [SerializeField]
        private AttackHitBox[] _hitBoxes;

        private float _elapsed = 0;
        private ComboInput _lastInput = ComboInput.None;

        public bool IsComboInputEnabled => _elapsed >= _comboInputAcceptanceTime && _elapsed < _comboInputDeactivationTime;
        public override string AnimationName => _animationName;

        private void OnEnable()
        {
            GizmoDrawer.OnDrawGizmosEvent += OnDrawGizmos;
        }

        private void OnDisable()
        {
            GizmoDrawer.OnDrawGizmosEvent -= OnDrawGizmos;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_hitBoxes == null) return;
            foreach (var hitBox in _hitBoxes)
            {
                if (hitBox == null) continue;
                hitBox.DrawGizmos(_elapsed, LayerMask);
            }
#endif
        }

        public override void Enter(PlayerController player)
        {
            _elapsed = 0f;
            _lastInput = ComboInput.None;

            foreach (var hitBox in _hitBoxes)
            {
                hitBox.Clear();
                hitBox.Center = player.transform;
            }
        }

        public override void Execute(PlayerController player)
        {
            var groundSensorResult = player.Sensor.Calculate(player);

            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;

            // アニメーションの進行度に応じてプレイヤーを移動させる
            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var xAxisMovement = (_xAxisMovementCurve.Evaluate(_elapsed) - _xAxisMovementCurve.Evaluate(_elapsed - Time.deltaTime));
            var groundNormal = groundSensorResult.GroundNormal;
            player.CharacterController.Move(Vector3.ProjectOnPlane(Vector3.right * sign, groundNormal) * xAxisMovement);

            var yAxisMovement = (_yAxisMovementCurve.Evaluate(_elapsed) - _yAxisMovementCurve.Evaluate(_elapsed - Time.deltaTime));
            player.CharacterController.Move(Vector3.up * yAxisMovement);

            foreach (var hitBox in _hitBoxes)
            {
                var additionalAttackPower = _baseAttackPower;
                var multiplierAttackPower = player.CharacterStats.AttackPower * _attackPowerFactor;
                hitBox.Update(additionalAttackPower, multiplierAttackPower, _elapsed, LayerMask);
            }
            if (IsComboInputEnabled)
            {
                // コンボ入力を受け付ける
                if (PlayerInputHandler.InGameInput.AttackX.triggered) _lastInput = ComboInput.X;
                else if (PlayerInputHandler.InGameInput.AttackY.triggered) _lastInput = ComboInput.Y;
            }
            if (previousElapsed < _nextAttackTransitionTime && _elapsed >= _nextAttackTransitionTime &&
                _lastInput != ComboInput.None)
            {
                // 次の攻撃に遷移する
                if (_lastInput == ComboInput.X) OnTransitionX?.Invoke(player);
                else if (_lastInput == ComboInput.Y) OnTransitionY?.Invoke(player);
            }
            if (previousElapsed < _attackCompletionTime && _elapsed >= _attackCompletionTime)
            {
                // 攻撃を完了する
                OnCompleted?.Invoke(player);
            }
        }

        public override void Exit(PlayerController player)
        {

        }
    }
}