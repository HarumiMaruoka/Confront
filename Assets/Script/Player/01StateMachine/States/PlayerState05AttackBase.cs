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
    public class PlayerState05AttackBase : PlayerState00Base
    {
        // 全ての攻撃ステートに共通して必要なデータと機能を記述してください。
    }
    // コンボを使用する攻撃クラスの基底クラス
    public class PlayerState05AttackBaseTypeCombo : PlayerState05AttackBase
    {
        [SerializeField]
        protected int _maxComboNumber = 0;
        public int CurrentComboNumber { get; protected set; } = 1;

        protected string[] _comboAnimParamName = null;

        public override void Init(PlayerStateMachine stateMachine)
        {
            base.Init(stateMachine);
            _comboAnimParamName = _stateMachine.AttackStateController.ComboParamName;
        }
        /// <summary> 攻撃ステートの開始処理 </summary>
        public override void Enter()
        {
            CurrentComboNumber = 1;
        }
        public override void Exit()
        {
            _stateMachine.PlayerController.Animator.
                       SetBool(_comboAnimParamName[CurrentComboNumber], false);
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
                UpdateCombo();
            }
        }
        protected void UpdateCombo()
        {
            // 攻撃1,あるいは2ボタンが押下されたとき次のコンボを再生する。
            if (_stateMachine.PlayerController.Input.IsAttack1InputButtonDown() ||
                _stateMachine.PlayerController.Input.IsAttack2InputButtonDown())
            {
                // 範囲内かチェック
                if (_maxComboNumber <= CurrentComboNumber) // 範囲内の場合
                {
                    // アニメーションを切り替える
                    _stateMachine.PlayerController.Animator.
                        SetBool(_comboAnimParamName[CurrentComboNumber], false);
                    CurrentComboNumber++;
                    _stateMachine.PlayerController.Animator.
                        SetBool(_comboAnimParamName[CurrentComboNumber], true);
                }
                else // 範囲外の場合
                {
                    Debug.LogWarning("範囲外が指定されました！修正してください！");
                }
            }
        }
    }
    // コンボを使用するMidair攻撃クラスの基底クラス
    public class PlayerState05AttackBaseTypeComboOnMidair : PlayerState05AttackBaseTypeCombo
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
                UpdateCombo();
            }
        }
    }
}