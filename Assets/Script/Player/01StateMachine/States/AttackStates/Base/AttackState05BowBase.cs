using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Player
{
    /// <summary>
    /// 弓攻撃ステートのベースクラス
    /// </summary>
    public class AttackState05BowBase : PlayerState05AttackBase
    {
        private AttackState _currentState = default;

        public override void Enter()
        {
            // 弾を1つ以上所持しているとき
            //   構えるアニメーションを再生する
            if (true/**/)
            {
                ChangeAnimation(0, AttackState.HoldWeapon);
            }
            // 弾を1つも所持していないとき
            // 他のステートに遷移する
            else
            {
                Transition();
            }
        }
        public override void Exit()
        {
            // このステートの初期化処理を記述する
            _currentState = AttackState.NotSet;
        }
        public override async void Update()
        {
            switch (_currentState)
            {
                case AttackState.HoldWeapon:
                    await HoldWeapon();
                    break;
                case AttackState.Shoot:
                    await Shoot();
                    break;
                case AttackState.UnarmWeapon:
                    UnarmWeapon();
                    break;
            }
        }
        private async UniTask HoldWeapon()
        {
            // 両方のボタンが離されるまで待つ
            await UniTask.WaitUntil(() =>
                !(_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                  _stateMachine.PlayerController.Input.IsAttack2InputButton()));
            // 離されたとき撃つステートに遷移する
            ChangeAnimation(1, AttackState.Shoot);
        }
        private async UniTask Shoot()
        {
            // 弾の所持数を減らす（アイテムの実装が完了したら実装する）

            // 攻撃アニメーションが再生し終わるまで待機
            await UniTask.WaitUntil(() =>
            _stateMachine.PlayerController.IsAnimEnd(AnimType.Attack));
            // 攻撃アニメーションの再生が完了したら収めるステートに遷移する
            ChangeAnimation(2, AttackState.UnarmWeapon);
        }
        private void UnarmWeapon()
        {
            // 武器を解くアニメーションの再生が完了したら通常ステートに遷移する
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                Transition();
            }
            // いずれかの攻撃ボタンが押されたら
            // もう一度撃つステートに遷移する
            if (_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                  _stateMachine.PlayerController.Input.IsAttack2InputButton())
            {
                ChangeAnimation(1, AttackState.Shoot);
            }
        }
        private void ChangeAnimation(int number, AttackState nextState)
        {
            ChangeAnimation(number);
            _currentState = nextState;
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
    /// <summary>
    /// 空中弓攻撃の基底クラス
    /// </summary>
    public class AttackState05MidairBowBase : AttackState05BowBase, IMidairAttack
    {

    }
}
