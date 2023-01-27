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
        private string _cancelButtonName = null;

        [SerializeField]
        private GameObject _weapon = default;

        public override void Init(PlayerStateMachine stateMachine, AttackStateManager attackStateController)
        {
            base.Init(stateMachine, attackStateController);
            // キャンセルボタンを設定する（今回は, トークボタンを割り当てる。）
            _cancelButtonName = _stateMachine.PlayerController.Input.TalkButtonName;
        }

        public override void Enter()
        {
            // 弾を1つ以上所持しているとき
            if (true /* ここに弾の数を判定する処理を書く */)
            {
                // 武器をアクティブにする。
                // _weapon?.SetActive(true);
                // 構えるアニメーションを再生する
                ChangeAnimation((int)AnimNumber.Equip, AttackState.Equipment);
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
            // 武器を非アクティブにする。
            //  _weapon?.SetActive(false);
            // このステートの初期化処理を記述する
            _currentState = AttackState.NotSet;
        }
        public override async void Update()
        {
            switch (_currentState)
            {
                case AttackState.Equipment:
                    await HoldWeapon();
                    break;
                case AttackState.Shoot:
                    await Shoot();
                    break;
                case AttackState.Unarm:
                    UnarmWeapon();
                    break;
            }
        }
        private async UniTask HoldWeapon()
        {   // 両方のボタンが離されるまで待つ
            // （待っている間にキャンセルボタンが押された時の事を想定して,
            // CancellationTokenSourceを渡し、キャンセル時処理を行うようにする。）
            await _stateMachine.PlayerController.IsAnimEndAsync(AnimType.HoldWeapon, default);
            await UniTask.WaitUntil(() =>
                !((_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                  _stateMachine.PlayerController.Input.IsAttack2InputButton())));
            // 離されたとき撃つステートに遷移する
            ChangeAnimation((int)AnimNumber.Recoil, AttackState.Shoot);
        }
        private async UniTask Shoot()
        {
            // 弾の所持数を減らす（アイテムの実装が完了したら実装する）

            // 攻撃アニメーションが再生し終わるまで待機
            await UniTask.WaitUntil(() =>
            _stateMachine.PlayerController.IsAnimEnd(AnimType.Attack));
            // 攻撃アニメーションの再生が完了したら収めるステートに遷移する
            ChangeAnimation((int)AnimNumber.Disarm, AttackState.Unarm);
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
                ChangeAnimation((int)AnimNumber.Aim, AttackState.Shoot);
            }
        }
        private void ChangeAnimation(int orderNumber, AttackState nextState)
        {
            ChangeAnimation(orderNumber);
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
        public enum AnimNumber : int
        {
            Equip = 0,
            Aim,
            Recoil,
            Disarm,
        }
    }
    /// <summary>
    /// 空中弓攻撃の基底クラス
    /// </summary>
    public class AttackState05MidairBowBase : AttackState05BowBase, IMidairAttack
    {

    }
}
