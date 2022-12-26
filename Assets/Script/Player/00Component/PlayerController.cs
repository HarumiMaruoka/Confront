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

        private CharacterController _characterController = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;

        private void Start()
        {
            _stateMachine.Init(this);
        }
        private void Update()
        {
            _stateMachine.Update();
        }
    }
}