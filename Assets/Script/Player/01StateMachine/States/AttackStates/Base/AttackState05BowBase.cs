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
        /// <summary> このステート内のステート </summary>
        private AttackState _currentState = default;

        [Tooltip("このステート中の移動加速度"), SerializeField]
        private float _moveAcceleration = default;
        // 移動加速度を元に戻すように保存するフィールド
        private float _saveAccelerationValue = default;

        [Tooltip("このステート中の最大移動速度"), SerializeField]
        private float _maxMoveSpeed = default;
        // 最大移動速度を元に戻すように保存するフィールド
        private float _saveMaxMoveSpeedValue = default;

        [Tooltip("矢の初速度の加算割合"), SerializeField, Range(1f, 10f)]
        private float _shootArrowPowerAddition = 1f;
        private float _maxShootArrowPower = 9999f;
        [Tooltip("インスペクタで確認する用"), SerializeField]
        private float _currentShootArrowPower = 0f;
        public float CurrentShootArrowPower => _currentShootArrowPower;

        public override void Init(PlayerStateMachine stateMachine, AttackStateManager attackStateController)
        {
            base.Init(stateMachine, attackStateController);
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
                // 歩行可能にし、アニメーションを制御する。
                _stateMachine.PlayerController.CanMove = true;
                RunWhileAttacking();
                // 移動速度を変更する / 元の移動速度を保存する
                _saveAccelerationValue = _stateMachine.PlayerController.MoveHorizontalAcceleration;
                _stateMachine.PlayerController.MoveHorizontalAcceleration = _moveAcceleration;

                _saveMaxMoveSpeedValue = _stateMachine.PlayerController.MaxMoveSpeedHorizontal;
                _stateMachine.PlayerController.MaxMoveSpeedHorizontal = _maxMoveSpeed;
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

            // 移動不可にする
            _stateMachine.PlayerController.CanMove = false;
            // 移動速度を元に戻す
            _stateMachine.PlayerController.MoveHorizontalAcceleration = _saveAccelerationValue;
            _stateMachine.PlayerController.MaxMoveSpeedHorizontal = _saveMaxMoveSpeedValue;
        }
        public override void Update()
        {
            RunWhileAttacking();
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
                    _stateMachine.PlayerController.Input.IsAttack2InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack3InputButton())
                {
                    _currentState = AttackState.AimIdle;
                    _currentShootArrowPower = 0f;
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
            // Debug.Log("I aim Now");
            // 矢の初速度を加算していくｩ!
            _currentShootArrowPower += Time.deltaTime * _shootArrowPowerAddition;
            // 上限を超えないようにする。
            if (_currentShootArrowPower > _maxShootArrowPower) _currentShootArrowPower = _maxShootArrowPower;
            // 攻撃ボタンが開放されとき、Shootへ遷移する。
            if (!_stateMachine.PlayerController.Input.IsAttack1InputButton() &&
                !_stateMachine.PlayerController.Input.IsAttack2InputButton() &&
                !_stateMachine.PlayerController.Input.IsAttack3InputButton())
            {
                _currentState = AttackState.Shoot;
                ChangeAnimation(2);
                return;
            }
        }
        private void Shoot()
        {
            // Debug.Log("I shoot Now");
            // Shootアニメーションの再生が完了したとき
            // 攻撃ボタンが押下されていたらAimへ
            // そうでなければ UnarmWeaponへ遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.Attack))
            {
                if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack2InputButton() ||
                    _stateMachine.PlayerController.Input.IsAttack3InputButton())
                {
                    _currentState = AttackState.AimIdle;
                    _currentShootArrowPower = 0f;
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
            // 攻撃ボタンが押下されたときAimへ遷移する。
            if (_stateMachine.PlayerController.Input.IsAttack1InputButton() ||
                _stateMachine.PlayerController.Input.IsAttack2InputButton() ||
                _stateMachine.PlayerController.Input.IsAttack3InputButton())
            {
                _currentState = AttackState.AimIdle;
                _currentShootArrowPower = 0f;
                ChangeAnimation(1);
                return;
            }
            // Debug.Log("I disaim Now");
            // 武装解除アニメーションの再生が完了したとき
            // 他のステートに遷移する。
            if (_stateMachine.PlayerController.IsAnimEnd(AnimType.UnarmWeapon))
            {
                Transition();
                return;
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
