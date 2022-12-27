using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Input _input = default;
        [SerializeField]
        private PlayerStateMachine _stateMachine = default;
        [SerializeField]
        private OverLapBox _groundChecker = default;

        private CharacterController _characterController = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public OverLapBox GroundChecker => _groundChecker;

        private void Start()
        {
            _stateMachine.Init(this);
            _groundChecker.Init(transform);
        }
        private void Update()
        {
            _stateMachine.Update();
        }
    }
}