﻿using Confront.AttackUtility;
using Confront.Input;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "ChargedAttackState", menuName = "ConfrontSO/Player/Combo/ChargedAttackState")]
    public class ChargedAttackState : AttackBehaviour
    {
        [Header("Movement Curve")]
        [SerializeField]
        private AnimationCurve _readyXAxisMovementCurve;
        [SerializeField]
        private AnimationCurve _readyYAxisMovementCurve;
        [Space]
        [SerializeField]
        private AnimationCurve _fireXAxisMovementCurve;
        [SerializeField]
        private AnimationCurve _fireYAxisMovementCurve;

        [Header("Animation Name")]
        [SerializeField]
        private string _readyAnimationName = "None";
        [SerializeField]
        private string _holdAnimationName = "None";
        [SerializeField]
        private string _fireAnimationName = "None";

        [Header("Time")]
        [SerializeField]
        private float _readyTime = 0.2f; // 準備時間
        [SerializeField]
        private float _holdTime = 0.5f; // チャージが最大になる時間
        [Space]
        [SerializeField]
        private float _nextAttackTransitionTime; // 次の攻撃に遷移する時間
        [SerializeField]
        private float _defaultStateTransitionTime; // デフォルト状態に遷移する時間
        [Space]
        [SerializeField]
        private float _nextAttackInputBeginTime; // 次の攻撃入力を受け付ける時間
        [SerializeField]
        private float _nextAttackInputEndTime; // 次の攻撃入力を無効にする時間

        [Header("SFX")]
        [SerializeField]
        private int _defaultHitSfxIndex = 0;
        [SerializeField]
        private SwingSfx[] _swingSfxes;

        [Header("Can Move")]
        [SerializeField]
        private bool _canMoveWhileCharging = false; // チャージ中に移動できるか

        [Space]
        [SerializeField]
        private ChargedHitBox[] _hitBoxes;

        [Space]
        [SerializeField]
        private ChargedShooter[] _shooters;

        private ChargeState _state = ChargeState.Ready;

        private ComboInput _lastInput = ComboInput.None;
        private float _elapsed = 0;
        private float _chargeAmount = 0; // 0 ~ 1

        public override float AnimationOffset => 0f;
        public override string AnimationName => string.Empty;

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneUnloaded += OnSceneLoaded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneUnloaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene)
        {
            _elapsed = 0f;
        }

        public override void Enter(PlayerController player, ComboTree tree)
        {
            _lastInput = ComboInput.None;
            _state = ChargeState.Ready;
            _elapsed = 0;
            Clear(tree);
            player.Animator.CrossFade(_readyAnimationName, 0.1f);
        }

        public override void Execute(PlayerController player, ComboTree tree)
        {
            switch (_state)
            {
                case ChargeState.Ready: HandleReadyState(player); break;
                case ChargeState.Hold: HandleHoldState(player); break;
                case ChargeState.Fire: HandleFireState(player); break;
            }
        }

        public override void Exit(PlayerController player, ComboTree tree)
        {
            _lastInput = ComboInput.None;
            _state = ChargeState.Ready;
            _elapsed = 0;
        }

        #region State Handlers
        private void HandleReadyState(PlayerController player)
        {
            _elapsed += Time.deltaTime;
            SwiftAttackState.UpdatePlayerMovement(player, _readyXAxisMovementCurve, _readyYAxisMovementCurve, _elapsed);
            if (_elapsed >= _readyTime)
            {
                _state = ChargeState.Hold;
                player.Animator.CrossFade(_holdAnimationName, 0.1f);
                _elapsed = 0;
            }
        }

        private void HandleHoldState(PlayerController player)
        {
            _elapsed += Time.deltaTime;
            _chargeAmount = Mathf.Clamp01(_elapsed / _holdTime);

            var attackButtonPressed = PlayerInputHandler.InGameInput.AttackX.IsPressed() || PlayerInputHandler.InGameInput.AttackY.IsPressed();

            if (_canMoveWhileCharging)
            {
                Grounded.Move(player);
                var groundSensorResult = player.Sensor.CalculateGroundState(player);
                if (groundSensorResult.GroundType != GroundType.Ground)
                {
                    OnCompleted?.Invoke(player);
                }
            }

            if (!attackButtonPressed) // 攻撃ボタンが離されたら
            {
                _state = ChargeState.Fire;
                player.MovementParameters.Velocity = Vector2.zero;
                player.Animator.CrossFade(_fireAnimationName, 0.1f);
                _elapsed = 0;
            }
        }

        private void HandleFireState(PlayerController player)
        {
            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;
            SwiftAttackState.UpdatePlayerMovement(player, _fireXAxisMovementCurve, _fireYAxisMovementCurve, _elapsed);

            foreach (var hitBox in _hitBoxes)
            {
                var direction = player.DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
                hitBox.Update(player.transform, player.AttackPower, direction, previousElapsed, _chargeAmount, LayerMask, true);
            }
            foreach (var shooter in _shooters)
            {
                shooter.Update(player, previousElapsed, _chargeAmount);
            }

            SwiftAttackState.HandleSwingSFXPlayback(player, _elapsed, previousElapsed, _swingSfxes);

            // 次の攻撃入力を受け付ける
            if (_elapsed >= _nextAttackInputBeginTime && _elapsed < _nextAttackInputEndTime)
            {
                if (PlayerInputHandler.InGameInput.AttackX.IsPressed())
                {
                    _lastInput = ComboInput.X;
                }
                else if (PlayerInputHandler.InGameInput.AttackY.IsPressed())
                {
                    _lastInput = ComboInput.Y;
                }
            }

            if (previousElapsed < _nextAttackTransitionTime && _elapsed >= _nextAttackTransitionTime && _lastInput != ComboInput.None)
            {
                if (_lastInput == ComboInput.X) OnTransitionX?.Invoke(player);
                else if (_lastInput == ComboInput.Y) OnTransitionY?.Invoke(player);
            }
            if (previousElapsed < _defaultStateTransitionTime && _elapsed >= _defaultStateTransitionTime)
            {
                OnCompleted?.Invoke(player);
            }
        }


        private void Clear(ComboTree tree)
        {
            foreach (var hitBox in _hitBoxes)
            {
                hitBox.Clear();
                if (!hitBox._hitSFX) hitBox._hitSFX = tree.DefaultHitSFX(_defaultHitSfxIndex);
                if (hitBox._hitVFX == null || !hitBox._hitVFX.VFXPrefab) hitBox._hitVFX = tree.DefaultVFX;

            }
            foreach (var shooter in _shooters)
            {
                shooter.Reset();
            }
        }

        private static PlayerController _player;

        protected override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_hitBoxes == null) return;
            foreach (var hitBox in _hitBoxes)
            {
                if (hitBox == null) continue;
                if (_player == null) _player = FindAnyObjectByType<PlayerController>();

                // ゲーム中のみ表示かつ有効時間のみ表示の場合、
                if (hitBox.GizmoOption == GizmoOption.RuntimeAndHitDetectionOnlyVisible)
                {
                    // Fire状態のみ表示する
                    if (_state != ChargeState.Fire) continue;
                }
                hitBox.DrawGizmos(_player.transform, _elapsed, LayerMask);
            }
#endif
        }
        #endregion

        public enum ChargeState
        {
            Ready,
            Hold,
            Fire,
        }
    }
}