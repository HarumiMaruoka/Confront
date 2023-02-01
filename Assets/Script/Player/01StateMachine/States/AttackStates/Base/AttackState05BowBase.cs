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
                ChangeAnimation(0);
                _currentState = AttackState.Equipment;
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
                case AttackState.NotSet: Debug.LogError("想定していない値が渡されました！修正してください！"); break;
                case AttackState.Equipment: Equipment(); break;
                case AttackState.AimIdle: Aim(); break;
                case AttackState.Shoot: Shoot(); break;
                case AttackState.Unarm: UnarmWeapon(); break;
            }
        }
        private void Equipment()
        {
            // 構えるアニメーションの再生が完了したとき
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.HoldWeapon))
            {
                // 攻撃ボタンが押下されていたらAimへ, そうでなければShootへ遷移する。
                if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButton())
                {
                    _currentState = AttackState.AimIdle;
                    ChangeAnimation(1);
                }
                else
                {
                    _currentState = AttackState.Shoot;
                    ChangeAnimation(2);
                }
            }
        }
        private void Aim()
        {
            // 攻撃ボタンが開放されとき、Shootへ遷移する。
            if (!_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                !_stateMachine.PlayerController.Input.IsAttack2InputButton())
            {
                _currentState = AttackState.Shoot;
                ChangeAnimation(2);
            }
        }
        private void Shoot()
        {
            // Shootアニメーションの再生が完了したとき
            // 攻撃ボタンが押下されていたらAimへ
            // そうでなければ UnarmWeaponへ遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButton())
                {
                    _currentState = AttackState.AimIdle;
                    ChangeAnimation(1);
                }
                else
                {
                    _currentState = AttackState.Unarm;
                    ChangeAnimation(3);
                }
            }
        }
        private void UnarmWeapon()
        {
            // 武装解除アニメーションの再生が完了したとき
            // 他のステートに遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                Transition();
                return;
            }
            // 攻撃ボタンが押下されたときAimへ遷移する。
            if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                _stateMachine.PlayerController.Input.IsAttack2InputButton())
            {
                _currentState = AttackState.AimIdle;
                ChangeAnimation(1);
            }
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
