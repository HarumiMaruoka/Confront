using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 剣攻撃ステートのベースクラス（槍やハンマーにも流用可）
    /// </summary>
    public class AttackState05SwordBase : PlayerState05AttackBase
    {
        [Tooltip("この攻撃による最大コンボ数を表現する値"), SerializeField]
        protected int _maxComboNumber = 0;

        public int MaxComboNumber => _maxComboNumber;

        [SerializeField]
        private GameObject _weapon = default;

        public override void Enter()
        {
            // 最初のアニメーションを再生する
            ChangeAnimation(CurrentAnimOrderNumber);
            // 武器をアクティブにする。
            // _weapon?.SetActive(true);
        }
        public override void Update()
        {
            // 現在再生中のコンボアニメーションナンバーと,現在ステートのコンボナンバーが一致し
            // 攻撃アニメーションの再生が完了した時, 状態を遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack) &&
                _stateMachine.PlayerController.Animator.
                GetInteger(_attackStateManager.OrderNumberAnimName) == CurrentAnimOrderNumber)
            {
                Transition();
                return;
            }

            // 攻撃入力が発生したとき
            // 次のコンボが存在するかチェックする
            // 次のコンボを再生する
            if (_stateMachine.PlayerController.Input.IsAttack1InputButtonDown() ||
                _stateMachine.PlayerController.Input.IsAttack2InputButtonDown())
            {
                if (CurrentAnimOrderNumber + 1 >= MaxComboNumber)
                {
                    CurrentAnimOrderNumber++;
                    ChangeAnimation(CurrentAnimOrderNumber);
                }
                else
                {
                    Debug.LogError("アニメーションを更新できません！");
                    Debug.LogError(
                        $"CurrentComboNumberは, {CurrentAnimOrderNumber}です！\n" +
                        $"MaxComboNumberは, {MaxComboNumber}です！");
                }
            }
        }
        public override void Exit()
        {
            // このステートの状態を初期化する。
            CurrentAnimOrderNumber = 0;
            // 武器を非アクティブにする。
            // _weapon?.SetActive(false);
        }
        protected override void Transition()
        {
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (!_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
            // 着地アニメーションの再生が終了したとき遷移処理を実行する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Land))
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
    }

    public class AttackState05MidairSwordBase : AttackState05SwordBase, IMidairAttack
    {
        public override void Enter()
        {
            // 最初のアニメーションを再生する
            ChangeAnimation(CurrentAnimOrderNumber);
            // 縦の移動計算を停止する
            _stateMachine.PlayerController.IsVerticalCalculation = false;
        }
        public override void Exit()
        {
            // このステートの状態を初期化する。
            CurrentAnimOrderNumber = 0;
            // 縦の移動計算を起動する
            _stateMachine.PlayerController.IsVerticalCalculation = true;
        }
    }
}