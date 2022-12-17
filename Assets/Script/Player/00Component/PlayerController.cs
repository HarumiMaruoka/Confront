using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerStateMachine _stateMachine;

        private CharacterController _characterController = null;


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