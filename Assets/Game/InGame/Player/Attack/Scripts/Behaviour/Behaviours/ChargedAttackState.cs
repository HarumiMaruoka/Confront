using Confront.AttackUtility;
using Confront.Input;
using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    [CreateAssetMenu(fileName = "ChargedAttackState", menuName = "ConfrontSO/Player/Combo/ChargedAttackState")]
    public class ChargedAttackState : AttackBehaviour
    {
        [Header("")]
        [SerializeField]
        private string _readyAnimationName = "None";
        [SerializeField]
        private string _holdAnimationName = "None";
        [SerializeField]
        private string _fireAnimationName = "None";

        [Header("")]
        [SerializeField]
        private float _readyTime = 0.2f; // 準備時間
        [SerializeField]
        private float _holdTime = 0.5f; // チャージが最大になる時間
        [SerializeField]
        private float _nextAttackTransitionTime; // 次の攻撃に遷移する時間
        [SerializeField]
        private float _defaultStateTransitionTime; // デフォルト状態に遷移する時間

        [Header("")]
        [SerializeField]
        private bool _canMoveWhileCharging = false; // チャージ中に移動できるか

        [Header("")]
        [SerializeField]
        private float _nextAttackInputBeginTime; // 次の攻撃入力を受け付ける時間
        [SerializeField]
        private float _nextAttackInputEndTime; // 次の攻撃入力を無効にする時間

        [SerializeField]
        private Shooter[] _shooters;

        private ChargeState _state = ChargeState.Ready;

        private ComboInput _lastInput = ComboInput.None;
        private float _elapsed = 0;
        private float _chargeAmount = 0; // 0 ~ 1

        public override string AnimationName => string.Empty;

        public override void Enter(PlayerController player)
        {
            _lastInput = ComboInput.None;
            _elapsed = 0;
            player.Animator.CrossFade(_readyAnimationName, 0.1f);
        }

        public override void Execute(PlayerController player)
        {
            switch (_state)
            {
                case ChargeState.Ready: HandleReadyState(player); break;
                case ChargeState.Hold: HandleHoldState(player); break;
                case ChargeState.Fire: HandleFireState(player); break;
            }
        }

        public override void Exit(PlayerController player)
        {

        }

        #region State Handlers
        private void HandleReadyState(PlayerController player)
        {
            _elapsed += Time.deltaTime;
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

            if (!attackButtonPressed) // 攻撃ボタンが離されたら
            {
                _state = ChargeState.Fire;
                player.Animator.CrossFade(_fireAnimationName, 0.1f);
                _elapsed = 0;
            }

            if (_canMoveWhileCharging)
            {
                Grounded.Move(player);
            }
        }

        private void HandleFireState(PlayerController player)
        {
            _elapsed += Time.deltaTime;

            foreach (var shooter in _shooters)
            {
                shooter.Update(player, _elapsed, _chargeAmount);
            }

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

            if (_elapsed >= _nextAttackTransitionTime && _lastInput != ComboInput.None)
            {
                if (_lastInput == ComboInput.X) OnTransitionX?.Invoke(player);
                else if (_lastInput == ComboInput.Y) OnTransitionY?.Invoke(player);
            }
            else if (_elapsed >= _defaultStateTransitionTime)
            {
                OnCompleted?.Invoke(player);
            }
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