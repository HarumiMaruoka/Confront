﻿using Confront.AttackUtility;
using Confront.Enemy.Slimey;
using Confront.GameUI;
using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy
{
    public class SlimeyController : EnemyBase, IDamageable
    {
        [Header("Components")]
        public Rigidbody Rigidbody;
        public Animator Animator;
        public SlimeyStats Stats;
        public EnemyEye Eye;

        [Header("States")]
        [SerializeField]
        private IdleState _idleState;
        [SerializeField]
        private WanderState _wanderState;
        [SerializeField]
        private ApproachState _approachState;
        [SerializeField]
        private AttackState _attackState;
        [SerializeField]
        private DamageState _damageState;
        [SerializeField]
        private DeadState _deadState;

        [Header("Utility")]
        public AttackHitBoxOneFrame AttackHitBox;
        public DirectionController DirectionController;

        [Header("For Checking (Do not modify from the editor)")]
        public SlimeyState CurrentState;

        private Dictionary<Type, SlimeyState> _states = new Dictionary<Type, SlimeyState>();
        private PlayerController _player;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            CurrentState.Execute(_player, this);
            DirectionController.UpdateVelocity(Rigidbody.velocity);
        }

        // Animation Event から呼び出す。
        public void Attack()
        {
            AttackHitBox.Clear();
            var sign = DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            AttackHitBox.Fire(transform, sign, Stats.AttackPower, PlayerLayerMask);
        }

        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            var damage = DefaultCalculateDamage(attackPower, Stats.Defense);

            Stats.Health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, transform.position);

            if (Stats.Health <= 0)
            {
                ChangeState<DeadState>();
            }
            else
            {
                ChangeState<DamageState>();
            }
        }

        public void ChangeState<T>() where T : SlimeyState
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_player, this);
            }
            CurrentState = _states[typeof(T)];
            CurrentState.Enter(_player, this);

            if (!string.IsNullOrEmpty(CurrentState.AnimationName))
            {
                Animator.CrossFade(CurrentState.AnimationName, 0.5f);
            }
        }

        private void OnDrawGizmos()
        {
            if (Eye != null) Eye.DrawGizmos(transform);

            AttackHitBox.DrawGizmos(transform, 0, PlayerLayerMask);
        }

        private void Initialize()
        {
            Stats = Instantiate(Stats);
            Eye = Instantiate(Eye);
            DirectionController.Initialize(transform);

            _player = PlayerController.Instance;
            if (_player == null) Debug.LogError("PlayerController is not found.");

            if (_idleState == null) Debug.LogError("IdleState is not found.");
            if (_wanderState == null) Debug.LogError("WanderState is not found.");
            if (_approachState == null) Debug.LogError("ApproachState is not found.");
            if (_attackState == null) Debug.LogError("AttackState is not found.");
            if (_damageState == null) Debug.LogError("DamageState is not found.");
            if (_deadState == null) Debug.LogError("DeadState is not found.");

            _states.Add(typeof(IdleState), _idleState);
            _states.Add(typeof(WanderState), _wanderState);
            _states.Add(typeof(ApproachState), _approachState);
            _states.Add(typeof(AttackState), _attackState);
            _states.Add(typeof(DamageState), _damageState);
            _states.Add(typeof(DeadState), _deadState);
            ChangeState<IdleState>();
        }
    }
}