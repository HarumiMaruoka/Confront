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
        private PlayerState05Attack _attack1 = default;
        [SerializeField]
        private PlayerState05Attack _attack2 = default;
        [SerializeField]
        private PlayerState06Damage _damage = default;
        [SerializeField]
        private PlayerState07Talk _talk = default;

        private PlayerController _playerController = null;

        public PlayerState01Idle Idle => _idle;
        public PlayerState02Move Move => _move;
        public PlayerState03Jump Jump => _jump;
        public PlayerState04Midair Midair => _midair;
        public PlayerState05Attack Attack1 => _attack1;
        public PlayerState05Attack Attack2 => _attack2;
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