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
        //[SerializeField]
        //private AttackState00BasicGunFire _basicGunFire = default;
        //[SerializeField]
        //private AttackState01BasicBow _attackState01BasicBow = default;
        //[SerializeField]
        //private AttackState01BasicBowMidair _attackState01BasicBowMidair = default;
        //[SerializeField]
        //private AttackState02NomalSword _attackState02NomalSword = default;
        //[SerializeField]
        //private AttackState02NomalSwordMidair _attackState02NomalSwordMidair = default;
        //[SerializeField]
        //private AttackState03LargeSword _attackState03LargeSword = default;
        //[SerializeField]
        //private AttackState03LargeSwordMidair _attackState03LargeSwordMidair = default;
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
            //InitLandState(_basicGunFire);
            //InitMidairState(_basicGunFire);
            //// 01
            //InitLandState(_attackState01BasicBow);
            //InitMidairState(_attackState01BasicBowMidair);
            //// 02
            //InitLandState(_attackState02NomalSword);
            //InitMidairState(_attackState02NomalSwordMidair);
            //// 03
            //InitLandState(_attackState03LargeSword);
            //InitMidairState(_attackState03LargeSwordMidair);


        }

        /// <summary>
        /// Landステート用 初期化処理
        /// </summary>
        public void InitLandState(PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine, this);
            if (targetState.MyID < 0 && targetState.MyID >= LandStates.Length)
            {
                Debug.LogError("設定されているIDが不正です！修正してください！");
            }
            else
            {
                LandStates[targetState.MyID] = targetState; ;
            }
        }

        /// <summary>
        /// Midairステート用 初期化処理
        /// </summary>
        public void InitMidairState(PlayerState05AttackBase targetState)
        {
            if (targetState.MyID < 0 && targetState.MyID >= MidairStates.Length)
            {
                Debug.LogError("設定されているIDが不正です！修正してください！");
            }
            else
            {
                MidairStates[targetState.MyID] = targetState; ;
            }
        }

        /// <summary>
        /// 引数に渡された値を攻撃ステートに設定する。
        /// </summary>
        /// <param name="attack1"> 攻撃ボタン1用ステート </param>
        /// <param name="attack2"> 攻撃ボタン2用ステート </param>
        private void SetFirstState(PlayerState05AttackBase attack1, PlayerState05AttackBase attack2)
        {
            _stateMachine.Attack1 = attack1;
            _stateMachine.Attack2 = attack2;
        }
    }
}