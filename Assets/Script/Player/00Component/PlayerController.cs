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
        private Helper.OverLapBox _groundChecker = default;
        [SerializeField]
        private Helper.Raycast _talkChecker = default;

        private CharacterController _characterController = null;
        private Animator _animator = null;

        public Input Input => _input;
        public PlayerStateMachine StateMachine => _stateMachine;
        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Helper.OverLapBox GroundChecker => _groundChecker;
        public Helper.Raycast TalkChecker => _talkChecker;

        private void Start()
        {
            _stateMachine.Init(this);
            _groundChecker.Init(transform);
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }
        private void Update()
        {
            _stateMachine.Update();
        }

        public void Damage(float value, Vector3 knockBackDir, float knockBackPower, DamageType type)
        {
            Debug.LogWarning("ダメージは未実装です。");
        }
    }
    public enum DamageType
    {
        Big, Middle, Small
    }
}