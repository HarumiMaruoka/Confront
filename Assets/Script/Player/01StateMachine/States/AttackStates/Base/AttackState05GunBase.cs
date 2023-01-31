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
        private AttackState _currentState = AttackState.NotSet;
        public override void Enter()
        {
            _stateMachine.PlayerController.IsReadyJump = false;
            // 弾を1つ以上所持している時
            //   銃を構えるアニメーションを再生する。
            //   弾を1つ減らす。
            if (true /* 弾の所持数の判定は省略 */ )
            {
                RunWhileAttacking();
                _stateMachine.PlayerController.CanMove = true;
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
        [SerializeField]
        private int _attackIntervalMillisecond = 0;
        public override void Exit()
        {
            _timer = 0f;
            _stateMachine.PlayerController.CanMove = false;
            _stateMachine.PlayerController.IsReadyJump = true;
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
                // 武器をアクティブにする。
                _weapon?.SetActive(true);
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
        [Tooltip("発砲の間隔"), SerializeField]
        private float _fireInterval = 0.2f;
        private float _timer = 0f;
        private void Shoot()
        {
            // インターバルを経過する度に発砲処理を行う
            // ここに処理を記述する
            // ボタンが離されたら状態を遷移する
            if (_timer < _fireInterval)
            {
                _attackStateManager.AttackMethodGroup.AttackGun();
                _timer = 0f;
            }
            _timer += Time.deltaTime;
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
