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
        private PlayerState06Damage _damage = default;
        [SerializeField]
        private PlayerState07Talk _talk = default;

        private PlayerController _playerController = null;
        private PlayerState05AttackBase _attack1 = null;
        private PlayerState05AttackBase _attack2 = null;
        private PlayerState05AttackBase _midairAttack1 = null;
        private PlayerState05AttackBase _midairAttack2 = null;

        public PlayerState01Idle Idle => _idle;
        public PlayerState02Move Move => _move;
        public PlayerState03Jump Jump => _jump;
        public PlayerState04Midair Midair => _midair;
        public PlayerState05AttackBase Attack1 { get => _attack1; set => _attack1 = value; }
        public PlayerState05AttackBase Attack2 { get => _attack2; set => _attack2 = value; }
        public PlayerState05AttackBase MidairAttack1 { get => _midairAttack1; set => _midairAttack1 = value; }
        public PlayerState05AttackBase MidairAttack2 { get => _midairAttack2; set => _midairAttack2 = value; }
        public PlayerState06Damage Damage => _damage;
        public PlayerState07Talk Talk => _talk;
        public PlayerController PlayerController => _playerController;
        #endregion

        public void Init(PlayerController playerController)
        {
            _playerController = playerController;
            Initialize(_idle);
        }
        protected override void StateInit()
        {
            _idle.Init(this);
            _move.Init(this);
            _jump.Init(this);
            _midair.Init(this);
            _attack1.Init(this);
            _damage.Init(this);
            _talk.Init(this);
        }
    }
}