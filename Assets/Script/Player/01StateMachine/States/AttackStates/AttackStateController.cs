using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// たくさんある攻撃ステートを管理するクラス
    /// </summary>
    [System.Serializable]
    public class AttackStateController
    {
        // 各攻撃ステート
        [SerializeField]
        private AttackState00BasicGunFire _basicGunFire = default;
        [SerializeField]
        private AttackState01TwinSword _attackState01TwinSword = default;
        [SerializeField]
        private AttackState01TwinSwordMidair _attackState01TwinSwordMidair = default;

        #region Controller
        [AnimationParameter, SerializeField]
        private string _attackAnimParamName = default;
        public string AttackAnimParamName => _attackAnimParamName;
        [Tooltip("2コンボ目以降のコンボ用アニメーションパラメータ名を設定する")]
        [AnimationParameter, SerializeField]
        private string[] _comboParamName = default;
        public string[] ComboParamName => _comboParamName;

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
            InitLandState(_basicGunFire);
            InitMidairState(_basicGunFire);

            InitLandState(_attackState01TwinSword);
            InitMidairState(_attackState01TwinSwordMidair);
        }

        private int _initLandCounter = 0;
        /// <summary>
        /// 呼び出された順番で各ステートを配列に保存し、初期化処理を実行する。<br/>
        /// Landステート用
        /// </summary>
        public void InitLandState(PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine);
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
            targetState.Init(_stateMachine);
            MidairStates[_initMidairCounter] = targetState;
            _initMidairCounter++;
        }
    }
}