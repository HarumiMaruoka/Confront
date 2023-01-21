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
        [Tooltip("攻撃用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _attackAnimName = default;
        public string AttackAnimName => _attackAnimName;
        [Tooltip("ID指定用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _attackIDAnimName = default;
        public string AttackIDAnimName => _attackIDAnimName;
        [Tooltip("コンボ番号指定用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _comboNumberAnimName = default;
        public string ComboNumberAnimName => _comboNumberAnimName;
        [Tooltip("アニメーション更新命令用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _animUpdateTriggerAnimName = default;
        public string AnimUpdateTriggerAnimName => _animUpdateTriggerAnimName;

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

            // 各ステートの初期化,配列に登録する処理
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

        private void InitState(PlayerState05AttackBase landState, PlayerState05AttackBase midairState)
        {
            InitState(LandStates, landState);
            InitState(MidairStates, midairState);
        }
        private void InitState(PlayerState05AttackBase[] states, PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine, this);
            if (targetState.MyID < 0 && targetState.MyID >= states.Length)
            {
                Debug.LogError("設定されているIDが不正です！修正してください！");
            }
            else
            {
                if (MidairStates[targetState.MyID] != null)
                {
                    Debug.LogError("設定されているIDが重複してます！修正してください！");
                }
                else
                {
                    states[targetState.MyID] = targetState;
                }
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