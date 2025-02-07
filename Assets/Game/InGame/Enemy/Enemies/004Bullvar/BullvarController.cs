using Confront.AttackUtility;
using Confront.Enemy.Slimey;
using Confront.Player;
using Confront.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    public class BullvarController : EnemyBase
    {
        [Header("Utiliity")]
        public Animator Animator;
        public SlimeyStats Stats;
        public CharacterController CharacterController;
        public DirectionController DirectionController;

        [Header("States")]
        public Idle Idle;
        public Wander Wander;
        public Approach Approach;
        public Attack Attack;
        public Block Block;
        public Damage Damage;
        public Dead Dead;

        [Header("Attack")]
        public HitBoxOneFrame HitBox;

        private BullvarState _currentState = null;
        private Dictionary<Type, BullvarState> _states = new Dictionary<Type, BullvarState>();

        private void Start()
        {
            Initialized();
            ChangeState<Idle>();
        }

        private void Initialized()
        {
            _states.Add(typeof(Idle), Idle);
            _states.Add(typeof(Wander), Wander);
            _states.Add(typeof(Approach), Approach);
            _states.Add(typeof(Attack), Attack);
            _states.Add(typeof(Block), Block);
            _states.Add(typeof(Damage), Damage);
            _states.Add(typeof(Dead), Dead);

            DirectionController.Initialize(transform);
        }

        private void Update()
        {
            _currentState?.Execute(PlayerController.Instance, this);
        }

        public void ChangeState<T>()
        {
            ChangeState(_states[typeof(T)]);
        }

        public void ChangeState(Type type)
        {
            ChangeState(_states[type]);
        }

        public void ChangeState(BullvarState bullvarState)
        {
            _currentState?.Exit(PlayerController.Instance, this);
            _currentState = bullvarState;
            Animator.Play(bullvarState.AnimationName);
            _currentState.Enter(PlayerController.Instance, this);
        }

        public void ProccesAttack() // アニメーションイベントから呼び出す
        {
            HitBox.Fire(
                transform,
                DirectionController.CurrentDirection == Direction.Right ? 1 : -1, Stats.AttackPower,
                LayerUtility.PlayerLayerMask);
        }

        private void OnDrawGizmos()
        {
            HitBox.DrawGizmos(transform, 0, LayerUtility.PlayerLayerMask);
        }

        private void OnAnimatorMove()
        {
            // Do nothing.
        }

        protected override string CreateSaveData()
        {
            Debug.LogError("Not implemented");
            return "";
        }

        protected override void Load(string saveData)
        {

        }
    }
}