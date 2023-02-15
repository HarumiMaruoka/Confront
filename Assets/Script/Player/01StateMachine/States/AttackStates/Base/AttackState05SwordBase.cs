using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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

        private CancellationTokenSource _fadeOutCanceller = new CancellationTokenSource();

        public override void Init(PlayerStateMachine stateMachine, AttackStateManager attackStateController)
        {
            base.Init(stateMachine, attackStateController);
        }
        public async override void Enter()
        {
            // 最初のアニメーションを再生する
            ChangeAnimation(CurrentAnimOrderNumber);
            // 武器をアクティブにする。
            _weapon?.SetActive(true);
            WeaponAnimator?.SetBool("OnFadeIn", true);
            _fadeOutCanceller.Cancel();
            await UniTask.DelayFrame(1);
            WeaponAnimator?.SetBool("OnFadeIn", false);
        }
        private bool _isComboUpdate = false;
        public override void Update()
        {
            // 攻撃アニメーションの再生が完了した時, 状態を遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                if (_isComboUpdate)
                {
                    _isComboUpdate = false;
                    if (CurrentAnimOrderNumber + 1 < MaxComboNumber)
                    {
                        CurrentAnimOrderNumber++;
                        ChangeAnimation(CurrentAnimOrderNumber);
                    }
                    else
                    {
                        Debug.Log($"{CurrentAnimOrderNumber}, {MaxComboNumber}");
                        Transition();
                        return;
                    }
                }
                else
                {
                    Transition();
                    return;
                }
            }

            // 攻撃入力が発生したとき
            // 次のコンボが存在するかチェックする
            // 次のコンボを再生する
            if (!_isComboUpdate)
            {
                _isComboUpdate =
                    _stateMachine.PlayerController.Input.IsAttack1InputButtonDown() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButtonDown();
            }
        }
        public async override void Exit()
        {
            // このステートの状態を初期化する。
            CurrentAnimOrderNumber = 0;
            _isComboUpdate = false;

            WeaponAnimator?.SetBool("OnFadeOut", true);
            await UniTask.DelayFrame(1);
            WeaponAnimator?.SetBool("OnFadeOut", false);

        }
        protected override void Transition()
        {
            // 非接地状態が検出されたとき、ステートをMidairに遷移する。
            if (!_stateMachine.PlayerController.GroundChecker.IsHit())
            {
                _stateMachine.TransitionTo(_stateMachine.Midair);
                return;
            }
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
        public enum SwordType
        {
            /// <summary> 双剣 </summary>
            TwinSword,
            /// <summary> 単剣 </summary>
            NomalSword,
            /// <summary> 大剣 </summary>
            GreatSword,
        }
    }

    public class AttackState05MidairSwordBase : AttackState05SwordBase, IMidairAttack
    {
        public override void Enter()
        {
            // 縦の移動計算を停止する
            _stateMachine.PlayerController.IsVerticalCalculation = false;
            base.Enter();
        }
        public override void Exit()
        {
            // 縦の移動計算を起動する
            _stateMachine.PlayerController.IsVerticalCalculation = true;
            base.Exit();
        }
    }
}