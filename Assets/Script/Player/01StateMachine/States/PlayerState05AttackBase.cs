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
    public class PlayerState05AttackBase : IState
    {
        [NonSerialized]
        protected PlayerStateMachine _stateMachine = null;
        [NonSerialized]
        protected AttackStateManager _attackStateManager = null;

        [Tooltip("この攻撃による最大コンボ数を表現する値"), SerializeField]
        protected int _maxComboNumber = 0;
        [Tooltip("この攻撃ステートのIDNumber"), SerializeField]
        protected int _myID = -1;

        public int MaxComboNumber => _maxComboNumber;
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
    }
}