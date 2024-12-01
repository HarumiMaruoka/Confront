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

        [Header("当たり判定_ヒットボックス")]
        [SerializeField]
        private AttackHitBox _hitBox;
        [SerializeField]
        private GizmoOption _isGizmosVisible = GizmoOption.AlwaysVisible;

        [Header("当たり判定_タイミング")]
        [SerializeField]
        private float _hitDetectionActivationTime = 0.1f; // 当たり判定を有効にする時間
        [SerializeField]
        private float _hitDetectionDeactivationTime = 0.2f; // 当たり判定を無効にする時間

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

        [Header("デバッグ")]
        [SerializeField]
        private bool _showLog = false;

        private float _elapsed = 0;
        private ComboInput _lastInput = ComboInput.None;

        public bool IsHitDetectionEnabled => _elapsed >= _hitDetectionActivationTime && _elapsed < _hitDetectionDeactivationTime;
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
            var isRuntime = Application.isPlaying;
            var isHitDetectionEnabled = IsHitDetectionEnabled;

            if (_isGizmosVisible == GizmoOption.None) return;
            if (_isGizmosVisible == GizmoOption.RuntimeOnlyVisible && !isRuntime) return;
            if (_isGizmosVisible == GizmoOption.RuntimeAndHitDetectionOnlyVisible && (!isRuntime || !isHitDetectionEnabled)) return;

            if (!_hitBox.Center) _hitBox.Center = GameObject.FindGameObjectWithTag("Player").transform;
            Gizmos.color = _hitBox.IsOverlapping(LayerMask) ? Color.red : Color.blue;
            Gizmos.matrix = Matrix4x4.TRS(_hitBox.Position, _hitBox.Center.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _hitBox.Size);
#endif
        }

        public override void Enter(PlayerController player)
        {
            _elapsed = 0f;
            _lastInput = ComboInput.None;

            _hitBox.Clear();
            _hitBox.Center = player.transform;
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

            if (IsHitDetectionEnabled)
            {
                var attackPower = _baseAttackPower + player.CharacterStats.AttackPower * _attackPowerFactor;
                _hitBox.Update(attackPower, LayerMask);
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

            DebugLog(previousElapsed, _elapsed);
        }

        public override void Exit(PlayerController player)
        {

        }

        private void DebugLog(float previousElapsed, float elapsed)
        {
            if (!_showLog) return;

            if (previousElapsed < _hitDetectionActivationTime && elapsed >= _hitDetectionActivationTime)
            {
                // 当たり判定を有効にする
                Debug.Log("Hit Detection Enabled");
            }
            if (previousElapsed < _hitDetectionDeactivationTime && elapsed >= _hitDetectionDeactivationTime)
            {
                // 当たり判定を無効にする
                Debug.Log("Hit Detection Disabled");
            }
            if (previousElapsed < _comboInputAcceptanceTime && elapsed >= _comboInputAcceptanceTime)
            {
                // コンボ入力を受け付ける
                Debug.Log("Combo Input Enabled");
            }
            if (previousElapsed < _comboInputDeactivationTime && elapsed >= _comboInputDeactivationTime)
            {
                // コンボ入力を無効にする
                Debug.Log("Combo Input Disabled");
            }
            if (previousElapsed < _nextAttackTransitionTime && elapsed >= _nextAttackTransitionTime)
            {
                // 次の攻撃に遷移する
                Debug.Log("Next Attack Transition");
            }
            if (previousElapsed < _attackCompletionTime && elapsed >= _attackCompletionTime)
            {
                // 攻撃を完了する
                Debug.Log("Attack Completed");
            }
        }

        public enum GizmoOption
        {
            None,
            AlwaysVisible, // 常に表示
            RuntimeOnlyVisible, // 実行時のみ表示
            RuntimeAndHitDetectionOnlyVisible, // 実行時かつ有効時のみ表示
        }
    }
}