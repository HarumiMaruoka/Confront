using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerStateMachine : StateMachineBase
    {
        #region State
        [SerializeField]
        private PlayerState01Idle _idle = default;
        [SerializeField]
        private PlayerState02Move _move = default;
        [SerializeField]
        private PlayerState03Jump _jump = default;
        [SerializeField]
        private PlayerState04Midair _midair = default;
        [SerializeField]
        private PlayerState06BigDamage _bigDamage = default;
        [SerializeField]
        private PlayerState06MiddleDamage _middleDamage = default;
        [SerializeField]
        private PlayerState06SmallDamage _smallDamage = default;
        [SerializeField]
        private PlayerState07Talk _talk = default;
        [SerializeField]
        private PlayerState08Land _land = default;
        [SerializeField]
        private AttackStateController _attackStateController = default;

        private PlayerController _playerController = null;

        private PlayerState05AttackBase _attackToExecute1 = null;
        private PlayerState05AttackBase _attackToExecute2 = null;
        private PlayerState05AttackBase _midairAttackToExecute1 = null;
        private PlayerState05AttackBase _midairAttackToExecute2 = null;

        public PlayerState01Idle Idle => _idle;
        public PlayerState02Move Move => _move;
        public PlayerState03Jump Jump => _jump;
        public PlayerState04Midair Midair => _midair;
        public PlayerState05AttackBase Attack1 { get => _attackToExecute1; set => _attackToExecute1 = value; }
        public PlayerState05AttackBase Attack2 { get => _attackToExecute2; set => _attackToExecute2 = value; }
        public PlayerState05AttackBase MidairAttack1 { get => _midairAttackToExecute1; set => _midairAttackToExecute1 = value; }
        public PlayerState05AttackBase MidairAttack2 { get => _midairAttackToExecute2; set => _midairAttackToExecute2 = value; }
        public PlayerState06BigDamage BigDamage => _bigDamage;
        public PlayerState06MiddleDamage MiddleDamage => _middleDamage;
        public PlayerState06SmallDamage SmallDamage => _smallDamage;
        public PlayerState07Talk Talk => _talk;
        public PlayerState08Land Land => _land;
        public AttackStateController AttackStateController => _attackStateController;
        public PlayerController PlayerController => _playerController;
        #endregion

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            base.Initialize(_idle);

            OnStateChanged += (previousState, nextState) =>
            {
                // 前のステート独自のアニメーションパラメータ名をfalseにし、
                // 次のステート独自のアニメーションパラメータ名をtrueにする。
                _playerController.Animator?.SetBool(
                    (previousState as PlayerState00Base).AnimParameterName, false);
                _playerController.Animator?.SetBool(
                    (nextState as PlayerState00Base).AnimParameterName, true);
                // 攻撃ステートのアニメーションパラメータ名を設定する。
                _playerController.Animator?.SetBool(
                    AttackStateController.AttackAnimParamName, nextState is PlayerState05AttackBase);
            };
        }
        protected override void StateInit()
        {
            _idle.Init(this);
            _move.Init(this);
            _jump.Init(this);
            _midair.Init(this);
            _bigDamage.Init(this);
            _middleDamage.Init(this);
            _smallDamage.Init(this);
            _talk.Init(this);
            _land.Init(this);
        }
    }
}