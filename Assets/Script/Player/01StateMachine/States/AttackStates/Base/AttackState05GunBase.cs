using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 銃攻撃ステートのベースクラス
    /// </summary>
    public class AttackState05GunBase : PlayerState05AttackBase
    {
        [SerializeField]
        private GameObject _weapon = default;

        private AttackState _currentState = AttackState.NotSet;
        public override void Enter()
        {
            // 弾を1つ以上所持している時
            //   銃を構えるアニメーションを再生する。
            //   弾を1つ減らす。
            if (true /* 弾の所持数の判定は省略 */ )
            {
                RunWhileAttacking();
                _stateMachine.PlayerController.CamMove = true;
                // 武器をアクティブにする。
                // _weapon?.SetActive(true);
                // アニメーションを変更する
                ChangeAnimation(0);
                // ステートを変更する
                _currentState = AttackState.Equipment;
            }
            // そうでないとき 他のステートに遷移する。
            else
            {
                Transition();
                return;
            }
        }
        public override void Update()
        {
            switch (_currentState)
            {
                case AttackState.NotSet: Debug.LogError("想定していない値が渡されました！修正してください！"); break;
                case AttackState.Equipment: Equipment(); break;
                case AttackState.AimIdle: Debug.LogError("想定していない値が渡されました！修正してください！"); break;
                case AttackState.Shoot: Shoot(); break;
                case AttackState.Unarm: Unarm(); break;
            }
            RunWhileAttacking();
        }
        public override void Exit()
        {
            // 武器を非アクティブにする。
            // _weapon?.SetActive(false);

            _stateMachine.PlayerController.CamMove = false;
        }
        protected override void Transition()
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
        // ================ このステート内のステート ================ //
        private void Equipment()
        {
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.HoldWeapon))
            {
                if (!_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                    !_stateMachine.PlayerController.Input.IsAttack2InputButton())
                {
                    _currentState = AttackState.Unarm;
                    ChangeAnimation(2);
                }
                else
                {
                    _currentState = AttackState.Shoot;
                    ChangeAnimation(1);
                }
            }
        }
        private void Shoot()
        {
            // インターバルを経過する度に発砲処理を行う
            // ここに処理を記述する
            // ボタンが離されたら状態を遷移する
            if (!_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                !_stateMachine.PlayerController.Input.IsAttack2InputButton())
            {
                _currentState = AttackState.Unarm;
                ChangeAnimation(2);
            }
        }
        private void Unarm()
        {
            // 武装解除アニメーションの再生が完了したとき状態を遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                // アニメーション更新処理
                _stateMachine.PlayerController.Animator.
                    SetBool(_attackStateManager.IsAttackAnimEndAnimName, true);
                Observable.NextFrame()
                     .Subscribe(_ => _stateMachine.PlayerController.Animator.
                    SetBool(_attackStateManager.IsAttackAnimEndAnimName, false));
                // ステートを変更する
                Transition();
                return;
            }
        }
    }
    public class AttackState05MidairGunBase : AttackState05GunBase, IMidairAttack
    {

    }
}
