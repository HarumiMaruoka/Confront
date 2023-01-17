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
            // コンボナンバーを初期化
            CurrentComboNumber = 1;
            // 攻撃入力を停止（アニメーションイベントから入力復帰を指定してください）
            _stateMachine.PlayerController.SetAcceptingAttackInput(false);
            // アニメーションの再生処理
            // 攻撃のサブステートに遷移
            _stateMachine.PlayerController.Animator.
                       SetBool(_attackStateManager.AttackParamName, true);
            // 攻撃IDを指定
            _stateMachine.PlayerController.Animator.
                       SetInteger(_attackStateManager.AttackIDAnimParamName, _myID);

        }
        /// <summary> 攻撃ステートの終了処理 </summary>
        public virtual void Exit()
        {
            // アニメーションの停止処理
            _stateMachine.PlayerController.Animator.
                       SetBool(_attackStateManager.AttackParamName, false);
            // 攻撃インターバルの設定
            _stateMachine.PlayerController.WaitAttackInterval();
        }
        /// <summary> コンボアニメーションの更新処理 </summary>
        protected void UpdteComboAnim()
        {
            CurrentComboNumber++;
            _stateMachine.PlayerController.Animator.
                SetTrigger(_attackStateManager.ComboTriggerAnimParamName);
            _stateMachine.PlayerController.Animator.
                SetInteger(_attackStateManager.ComboCounterAnimParamName, CurrentComboNumber);
        }
        /// <summary> 毎フレーム実行する処理 </summary>
        public virtual void Update()
        {
            // アニメーション終了時, Attackアニメーションの完了が検知されたときステートを遷移する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
                else
                {
                    // 移動入力があるとき、ステートをMoveに遷移する。
                    if (_stateMachine.PlayerController.Input.IsMoveInput)
                    {
                        _stateMachine.TransitionTo(_stateMachine.Move);
                        return;
                    }
                    // 移動入力があるとき、ステートをMoveに遷移する。
                    else
                    {
                        _stateMachine.TransitionTo(_stateMachine.Idle);
                        return;
                    }
                }
            }
            // アニメーション再生中, 入力が発生したとき次のコンボを再生する
            else
            {
                // 攻撃1,あるいは2ボタンが押下されたとき次のコンボを再生する。
                if (_stateMachine.PlayerController.Input.IsAttack1InputButtonDown() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButtonDown())
                {
                    // 範囲内かチェック
                    if (_maxComboNumber <= CurrentComboNumber) // 範囲内の場合
                    {
                        /// 次のコンボアニメーションの再生処理。
                        UpdteComboAnim();
                    }
                    else // 範囲外の場合
                    {
                        Debug.LogWarning("範囲外が指定されました！修正してください！");
                    }
                }
            }
        }
    }
    public class PlayerState05AttackBaseOnMidair : PlayerState05AttackBase
    {
        public override void Enter()
        {
            base.Enter();
            _stateMachine.PlayerController.IsVerticalCalculation = false;
        }
        public override void Exit()
        {
            base.Exit();
            _stateMachine.PlayerController.IsVerticalCalculation = true;
        }
        public override void Update()
        {
            // アニメーション終了時, Attackアニメーションの完了が検知されたときステートを遷移する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                // 非接地状態が検出されたとき、ステートをMidairに遷移する。
                if (!_stateMachine.PlayerController.GroundChecker.IsHit())
                {
                    _stateMachine.TransitionTo(_stateMachine.Midair);
                    return;
                }
                // 接地状態であれば, ステートをLandに遷移する。
                else
                {
                    _stateMachine.TransitionTo(_stateMachine.Land);
                    return;
                }
            }
            // アニメーション再生中, 入力が発生したとき次のコンボを再生する
            else
            {
                // 攻撃1,あるいは2ボタンが押下されたとき次のコンボを再生する。
                if (_stateMachine.PlayerController.Input.IsAttack1InputButtonDown() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButtonDown())
                {
                    // 範囲内かチェック
                    if (_maxComboNumber <= CurrentComboNumber) // 範囲内の場合
                    {
                        /// 次のコンボアニメーションの再生処理。
                        UpdteComboAnim();
                    }
                    else // 範囲外の場合
                    {
                        Debug.LogWarning("範囲外が指定されました！修正してください！");
                    }
                }
            }
        }
    }
}