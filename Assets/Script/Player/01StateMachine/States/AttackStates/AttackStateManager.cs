using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// たくさんある攻撃ステートを管理するクラス
    /// </summary>
    [System.Serializable]
    public class AttackStateManager
    {
        // =================== 各攻撃ステート =================== //
        #region States
        [SerializeField]
        private AttackState00BasicGunFire _basicGunFire = default;
        [SerializeField]
        private AttackState01TwinSword _attackState01TwinSword = default;
        [SerializeField]
        private AttackState01TwinSwordMidair _attackState01TwinSwordMidair = default;
        [SerializeField]
        private AttackState02NomalSword _attackState02NomalSword = default;
        [SerializeField]
        private AttackState02NomalSwordMidair _attackState02NomalSwordMidair = default;
        [SerializeField]
        private AttackState03LargeSword _attackState03LargeSword = default;
        [SerializeField]
        private AttackState03LargeSwordMidair _attackState03LargeSwordMidair = default;
        #endregion

        #region Controller
        [Tooltip("攻撃開始用アニメーションパラメータ名を設定する")]
        [AnimationParameter, SerializeField]
        private string _attackParamName = default;
        public string AttackParamName => _attackParamName;
        [Tooltip("空中攻撃開始用アニメーションパラメータ名を設定する")]
        [AnimationParameter, SerializeField]
        private string _midairAttackParamName = default;
        public string MidairAttackParamName => _midairAttackParamName;
        [Tooltip("攻撃ID指定用intアニメーションパラメータ名を設定する")]
        [AnimationParameter, SerializeField]
        private string _attackIDAnimParamName = default;
        public string AttackIDAnimParamName => _attackIDAnimParamName;
        [Tooltip("コンボ更新用トリガー名を設定する")]
        [AnimationParameter, SerializeField]
        private string _comboTriggerAnimParamName = default;
        public string ComboTriggerAnimParamName => _comboTriggerAnimParamName;
        [Tooltip("現在何コンボ目かを表すintアニメーションパラメータ名を設定する")]
        [AnimationParameter, SerializeField]
        private string _comboCounterAnimParamName = default;
        public string ComboCounterAnimParamName => _comboCounterAnimParamName;

        [Tooltip("攻撃終了時から次の攻撃が可能になるまでのインターバル"), SerializeField]
        private float _attackInterval = 0.2f;
        public float AttackInterval => _attackInterval;

        public PlayerState05AttackBase[] LandStates { get; private set; } = new PlayerState05AttackBase[Constants.MaxWeaponID];
        public PlayerState05AttackBase[] MidairStates { get; private set; } = new PlayerState05AttackBase[Constants.MaxWeaponID];

        [NonSerialized]
        private PlayerStateMachine _stateMachine = null;
        #endregion

        public void Init(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            // ↓こんな感じで 各ステートを初期化し、配列に登録していく
            // ID順に初期化する必要があるので注意する
            // 00
            InitLandState(_basicGunFire);
            InitMidairState(_basicGunFire);
            // 01
            InitLandState(_attackState01TwinSword);
            InitMidairState(_attackState01TwinSwordMidair);
            // 02
            InitLandState(_attackState02NomalSword);
            InitMidairState(_attackState02NomalSwordMidair);
            // 03
            InitLandState(_attackState03LargeSword);
            InitMidairState(_attackState03LargeSwordMidair);
        }

        private int _initLandCounter = 0;
        /// <summary>
        /// 呼び出された順番で各ステートを配列に保存し、初期化処理を実行する。<br/>
        /// Landステート用
        /// </summary>
        public void InitLandState(PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine, this);
            LandStates[_initLandCounter] = targetState;
            _initLandCounter++;
        }

        private int _initMidairCounter = 0;
        /// <summary>
        /// 呼び出された順番で各ステートを配列に保存し、初期化処理を実行する。<br/>
        /// Midairステート用
        /// </summary>
        public void InitMidairState(PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine, this);
            MidairStates[_initMidairCounter] = targetState;
            _initMidairCounter++;
        }
    }
}