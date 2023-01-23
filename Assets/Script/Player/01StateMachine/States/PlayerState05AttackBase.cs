using System;
using UnityEngine;


namespace Player
{
    // 攻撃の仕様 : 
    // 武器の数だけ攻撃ステート（Land版とMidair版）を作成する。
    // 武器切り替え時に実行するステートを変更する。

    /// <summary>
    /// 全ての攻撃ステートの基底クラス
    /// </summary>
    [System.Serializable]
    public abstract class PlayerState05AttackBase : IState
    {
        [NonSerialized]
        protected PlayerStateMachine _stateMachine = null;
        [NonSerialized]
        protected AttackStateManager _attackStateManager = null;

        [Tooltip("この攻撃ステートのIDNumber"), SerializeField]
        protected int _myID = -1;

        public int MyID => _myID;

        /// <summary> 現在 何コンボ目を実行中か表す値 （0からカウントアップ） </summary>
        public int CurrentComboNumber { get; protected set; } = 0;

        public virtual void Init(PlayerStateMachine stateMachine, AttackStateManager attackStateController)
        {
            _stateMachine = stateMachine;
            _attackStateManager = attackStateController;
        }

        /// <summary> 攻撃ステートの開始処理 </summary>
        public virtual void Enter()
        {

        }
        /// <summary> 攻撃ステートの終了処理 </summary>
        public virtual void Exit()
        {

        }
        /// <summary> 攻撃ステートの更新処理 </summary>
        public virtual void Update()
        {

        }
        /// <summary>
        /// アニメーションの更新命令
        /// </summary>
        /// <param name="targetNumber"></param>
        protected void ChangeAnimation(int targetNumber)
        {
            _stateMachine.PlayerController.Animator.
                SetInteger(_attackStateManager.ComboNumberAnimName, targetNumber);
            _stateMachine.PlayerController.Animator.
                SetTrigger(_attackStateManager.AnimUpdateTriggerAnimName);
        }
        /// <summary>
        /// ステート遷移処理
        /// </summary> 
        protected virtual void Transition()
        {
            Debug.LogWarning("未実装");
        }
    }
    /// <summary>
    /// 攻撃の際にあったら便利そうな状態を表す列挙体
    /// </summary>
    public enum AttackState
    {
        /// <summary> 未設定 </summary>
        NotSet,
        /// <summary> 構える </summary>
        HoldWeapon,
        /// <summary> 撃つ </summary>
        Shoot,
        /// <summary> 収める </summary>
        UnarmWeapon,
    }
}