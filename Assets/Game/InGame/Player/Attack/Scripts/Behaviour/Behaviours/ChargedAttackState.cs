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
        private float _readyTime = 0.2f;
        [SerializeField]
        private float _holdTime = 0.5f;
        [SerializeField]
        private float _fireTime = 0.3f;

        [Header("")]
        [SerializeField]
        private float _nextAttackInputBeginTime; // 次の攻撃入力を受け付ける時間
        [SerializeField]
        private float _nextAttackInputEndTime; // 次の攻撃入力を無効にする時間

        [Header("")]
        [SerializeField]
        private float _nextAttackTransitionTime; // 次の攻撃に遷移する時間
        [SerializeField]
        private float _defaultStateTransitionTime; // デフォルト状態に遷移する時間

        private ChargeState _state = ChargeState.Ready;

        private float _chargeAmount = 0; // 0 ~ 1

        private float _elapsed = 0;

        public override string AnimationName => string.Empty;

        public override void Enter(PlayerController player)
        {

        }

        public override void Execute(PlayerController player)
        {

        }

        public override void Exit(PlayerController player)
        {

        }

        public enum ChargeState
        {
            Ready,
            Hold,
            Fire,
        }
    }
}