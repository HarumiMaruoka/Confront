using Confront.Debugger;
using Confront.Input;
using Confront.AttackUtility;
using System;
using UnityEngine;


namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "SwiftAttackState", menuName = "ConfrontSO/Player/Combo/SwiftAttackState")]
    public class SwiftAttackState : AttackBehaviour
    {
        [Header("アニメーション")]
        [SerializeField]
        private string _animationName = "None";
        [SerializeField]
        private AnimationCurve _xAxisMovementCurve;
        [SerializeField]
        private AnimationCurve _yAxisMovementCurve;

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

        [Header("当たり判定")]
        [SerializeField]
        private AttackHitBox[] _hitBoxes;

        [Header("射出")]
        [SerializeField]
        private Shooter[] _shooters;

        private float _elapsed = 0;
        private ComboInput _lastInput = ComboInput.None;

        public bool IsComboInputEnabled => _elapsed >= _comboInputAcceptanceTime && _elapsed < _comboInputDeactivationTime;
        public override string AnimationName => _animationName;

        private static PlayerController _player;

        protected override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_hitBoxes == null) return;

            if (_player == null) _player = FindObjectOfType<PlayerController>();

            foreach (var hitBox in _hitBoxes)
            {
                if (hitBox == null) continue;
                hitBox.DrawGizmos(_player.transform, _elapsed, LayerMask);
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
            }
            foreach (var shooter in _shooters)
            {
                shooter.Reset();
            }
        }

        public override void Execute(PlayerController player)
        {

            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;

            UpdatePlayerMovement(player, _xAxisMovementCurve, _yAxisMovementCurve, _elapsed);
            UpdateHitBoxesAndShooters(player);
            HandleTransition(player, previousElapsed);
        }

        public static void UpdatePlayerMovement(PlayerController player, AnimationCurve xAxisMovementCurve, AnimationCurve yAxisMovementCurve, float elapsed)
        {
            // アニメーションの進行度に応じてプレイヤーを移動させる
            var groundSensorResult = player.Sensor.Calculate(player);
            var sign = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            var xAxisMovement = (xAxisMovementCurve.Evaluate(elapsed) - xAxisMovementCurve.Evaluate(elapsed - Time.deltaTime));
            var groundNormal = groundSensorResult.GroundNormal;
            var angle = Vector3.Angle(Vector3.up, groundNormal);
            if (angle > player.CharacterController.slopeLimit)
            {
                xAxisMovement = 0f;
            }

            player.CharacterController.Move(Vector3.ProjectOnPlane(Vector3.right * sign, groundNormal) * xAxisMovement);

            var yAxisMovement = (yAxisMovementCurve.Evaluate(elapsed) - yAxisMovementCurve.Evaluate(elapsed - Time.deltaTime));
            player.CharacterController.Move(Vector3.up * yAxisMovement);
        }

        private void UpdateHitBoxesAndShooters(PlayerController player)
        {
            if (_hitBoxes != null)
            {
                foreach (var hitBox in _hitBoxes)
                {
                    var direction = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                    hitBox.Update(player.transform, player.CharacterStats.AttackPower, direction, _elapsed, LayerMask);
                }
            }

            if (_shooters != null)
            {
                foreach (var shooter in _shooters)
                {
                    shooter.Update(player, _elapsed);
                }
            }
        }

        private void HandleTransition(PlayerController player, float previousElapsed)
        {
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