using System;
using UniRx;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// たくさんある攻撃ステートを管理するクラス
    /// </summary>
    [System.Serializable]
    public class AttackStateManager
    {
        #region Controller
        [SerializeField]
        private AttackMethodGroup _attackMethodGroup = default;
        public AttackMethodGroup AttackMethodGroup => _attackMethodGroup;
        [Tooltip("攻撃用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _attackAnimName = default;
        public string AttackAnimName => _attackAnimName;
        [Tooltip("空中攻撃用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _midairAttackAnimName = default;
        public string MidairAttackAnimName => _midairAttackAnimName;
        [Tooltip("種類指定用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _attackEnumAnimName = default;
        public string AttackEnumAnimName => _attackEnumAnimName;
        [Tooltip("コンボ番号指定用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _orderNumberAnimName = default;
        public string OrderNumberAnimName => _orderNumberAnimName;
        [Tooltip("アニメーション更新命令用パラメータ名を指定する。"), AnimationParameter, SerializeField]
        private string _animUpdateTriggerAnimName = default;
        public string AnimUpdateTriggerAnimName => _animUpdateTriggerAnimName;
        [Tooltip("攻撃終了時を知らせる為に使用するパラメータ名を指定してください。"), AnimationParameter, SerializeField]
        private string _isAttackAnimEndAnimName = default;
        public string IsAttackAnimEndAnimName => _isAttackAnimEndAnimName;
        [Tooltip("攻撃中に移動する際に使用するパラメータを設定してください。"), AnimationParameter, SerializeField]
        private string _isRunAnimName = default;
        public string IsRunAnimName => _isRunAnimName;

        [Tooltip("攻撃終了時から次の攻撃が可能になるまでのインターバル"), SerializeField]
        private float _attackInterval = 0.2f;
        public float AttackInterval => _attackInterval;

        public PlayerState05AttackBase[] LandStates { get; private set; } =
            new PlayerState05AttackBase[Constants.MaxWeaponID];
        public PlayerState05AttackBase[] MidairStates { get; private set; } =
            new PlayerState05AttackBase[Constants.MaxWeaponID];

        [NonSerialized]
        private PlayerStateMachine _stateMachine = null;
        #endregion

        // =================== 各攻撃ステート =================== //
        #region States
        [SerializeField]
        private AttackState00BasicGunFire _attackState00BasicGunFire = default;
        [SerializeField]
        private AttackState00MidairBasicGunFire _attackState00BasicGunFireMidair = default;
        [SerializeField]
        private AttackState01BasicBow _attackState01BasicBow = default;
        [SerializeField]
        private AttackState01BasicBowMidair _attackState01BasicBowMidair = default;
        [SerializeField]
        private AttackState02NomalSword _attackState02NomalSword = default;
        [SerializeField]
        private AttackState02NomalSwordMidair _attackState02NomalSwordMidair = default;
        [SerializeField]
        private AttackState03LargeSword _attackState03LargeSword = default;
        [SerializeField]
        private AttackState03LargeSwordMidair _attackState03LargeSwordMidair = default;

        public AttackState00BasicGunFire AttackState00BasicGunFire => _attackState00BasicGunFire;
        public AttackState00MidairBasicGunFire AttackState00BasicGunFireMidair => _attackState00BasicGunFireMidair;
        public AttackState01BasicBow AttackState01BasicBow => _attackState01BasicBow;
        public AttackState01BasicBowMidair AttackState01BasicBowMidair => _attackState01BasicBowMidair;
        public AttackState02NomalSword AttackState02NomalSword => _attackState02NomalSword;
        public AttackState02NomalSwordMidair AttackState02NomalSwordMidair => _attackState02NomalSwordMidair;
        public AttackState03LargeSword AttackState03LargeSword => _attackState03LargeSword;
        public AttackState03LargeSwordMidair AttackState03LargeSwordMidair => _attackState03LargeSwordMidair;
        #endregion

        public void Init(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            // 各ステートの初期化,配列に登録する処理
            // 00
            InitState(_attackState00BasicGunFire, _attackState00BasicGunFireMidair);
            // 01
            InitState(_attackState01BasicBow, _attackState01BasicBowMidair);
            // 02
            InitState(_attackState02NomalSword, _attackState02NomalSwordMidair);
            // 03
            InitState(_attackState03LargeSword, _attackState03LargeSwordMidair);
        }

        /// <summary>
        /// 渡されたIDをCurrentAttackStateとして設定する
        /// </summary>
        /// <param name="oneID"> 攻撃ボタン1用ステート </param>
        /// <param name="twoID"> 攻撃ボタン2用ステート </param>
        public void SetBothState(int oneID, int twoID)
        {
            SetOneState(oneID);
            SetTwoState(twoID);
        }
        /// <summary>
        /// 渡されたIDを使用する攻撃として設定する
        /// </summary>
        /// <param name="id"> 攻撃ボタン1用ステートID </param>
        public void SetOneState(int id)
        {
            _stateMachine.Attack1 = LandStates[id];
            _stateMachine.MidairAttack1 = MidairStates[id];
        }
        /// <summary>
        /// 渡されたIDを使用する攻撃として設定する
        /// </summary>
        /// <param name="id"> 攻撃ボタン2用ステートID </param>
        public void SetTwoState(int id)
        {
            _stateMachine.Attack2 = LandStates[id];
            _stateMachine.MidairAttack2 = MidairStates[id];
        }
        /// <summary>
        /// ステートの初期化処理
        /// </summary>
        /// <param name="landState"> 攻撃ステート（地上用） </param>
        /// <param name="midairState"> 攻撃ステート（空中用） </param>
        private void InitState(PlayerState05AttackBase landState, PlayerState05AttackBase midairState)
        {
            InitState(LandStates, landState);
            InitState(MidairStates, midairState);
        }
        /// <summary>
        /// 配列に登録する処理
        /// </summary>
        private void InitState(PlayerState05AttackBase[] states, PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine, this);
            if (targetState.MyID < 0 && targetState.MyID >= states.Length)
            {
                Debug.LogError("設定されているIDが不正です！修正してください！");
            }
            else
            {
                if (states[targetState.MyID] != null)
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
        /// 攻撃アニメーションの終了処理
        /// </summary>
        public void OnAttackAnimEnd()
        {
            _stateMachine.PlayerController.Animator.
                SetBool(this.IsAttackAnimEndAnimName, true);
            Observable.NextFrame()
                 .Subscribe(_ => _stateMachine.PlayerController.Animator.
                SetBool(this.IsAttackAnimEndAnimName, false));
        }
    }
}