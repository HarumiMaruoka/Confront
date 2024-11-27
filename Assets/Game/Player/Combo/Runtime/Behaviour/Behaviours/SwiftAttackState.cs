using Confront.Player.Combo;
using System;
using UnityEngine;


namespace Confront.Player
{
    [CreateAssetMenu(fileName = "SwiftAttackState", menuName = "Confront/Player/Combo/SwiftAttackState")]
    public class SwiftAttackState : AttackBehaviour
    {
        [SerializeField]
        private string _animationName = "None";

        [SerializeField]
        private float _hitDetectionActivationTime = 0.1f; // 当たり判定を有効にする時間
        [SerializeField]
        private float _hitDetectionDeactivationTime = 0.2f; // 当たり判定を無効にする時間

        [SerializeField]
        private float _comboInputAcceptanceTime = 0.2f; // コンボ入力を受け付ける時間
        [SerializeField]
        private float _comboInputDeactivationTime = 0.2f; // コンボ入力を無効にする時間

        [SerializeField]
        private float _nextAttackTransitionTime = 0.2f; // 次の攻撃に遷移する時間
        [SerializeField]
        private float _attackCompletionTime = 0.3f; // 攻撃を完了する時間

        [SerializeField]
        private bool _showLog = false;

        private float _elapsed = 0;

        public bool IsHitDetectionEnabled => _elapsed >= _hitDetectionActivationTime && _elapsed < _hitDetectionDeactivationTime;
        public bool IsComboInputEnabled => _elapsed >= _comboInputAcceptanceTime && _elapsed < _comboInputDeactivationTime;
        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player)
        {
            _elapsed = 0f;
        }

        public override void Execute(PlayerController player)
        {
            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;

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
                if (_showLog) Debug.Log("Hit Detection Enabled");
            }
            if (previousElapsed < _hitDetectionDeactivationTime && elapsed >= _hitDetectionDeactivationTime)
            {
                // 当たり判定を無効にする
                if (_showLog) Debug.Log("Hit Detection Disabled");
            }
            if (previousElapsed < _comboInputAcceptanceTime && elapsed >= _comboInputAcceptanceTime)
            {
                // コンボ入力を受け付ける
                if (_showLog) Debug.Log("Combo Input Enabled");
            }
            if (previousElapsed < _comboInputDeactivationTime && elapsed >= _comboInputDeactivationTime)
            {
                // コンボ入力を無効にする
                if (_showLog) Debug.Log("Combo Input Disabled");
            }
        }
    }
}