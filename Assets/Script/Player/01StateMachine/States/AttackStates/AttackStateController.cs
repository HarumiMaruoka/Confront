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
        // ↓こんな感じでステートをインスペクタウィンドウで操作可能にしつつ列挙していく
        // [SerializeField]
        // PlayerState05Attack00 basicPunch = default;
        // [SerializeField]
        // PlayerState05Attack01 basicGunFire = default;

        private PlayerState05AttackBase[] _states = new PlayerState05AttackBase[Constants.MaxWeaponID];
        public PlayerState05AttackBase[] States => _states;

        private PlayerStateMachine _stateMachine = null;
        public void Init(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            // ↓こんな感じで 各ステートを初期化し、配列に登録していく
            // InitSate(basicPunch);
            // InitSate(basicGunFire);
        }

        private int _initCounter = 0;
        /// <summary>
        /// 呼び出された順番で各ステートを配列に保存し、初期化処理を実行する。
        /// </summary>
        public void InitState(PlayerState05AttackBase targetState)
        {
            targetState.Init(_stateMachine);
            _states[_initCounter] = targetState;
            _initCounter++;
        }
    }
}