using System;
using UnityEngine;
using Confront.Enemy.VampireBat;
using Confront.AttackUtility;
using Confront.Player;
using System.Collections.Generic;
using Confront.GameUI;
using Confront.Utility;

namespace Confront.Enemy
{
    public class VampireBatController : EnemyBase, IDamageable
    {
        public Transform OrbitCenter;

        [Header("Components")]
        public Animator Animator;
        public VampireBatStats Stats;
        public EnemyEye Eye;

        [Header("States")]
        [SerializeField]
        private Idle _idleState;
        [SerializeField]
        private Fly _flyState;
        [SerializeField]
        private Approach _approachState;
        [SerializeField]
        private Attack _attackState;
        [SerializeField]
        private Damage _damageState;
        [SerializeField]
        private Die _dieState;

        [Header("Utility")]
        public HitBoxOneFrame AttackHitBox;
        public DirectionController DirectionController;
        public VampireBat.Sensor Sensor;

        [Header("For Checking (Do not modify from the editor)")]
        public VampireBatState CurrentState;

        private Dictionary<Type, VampireBatState> _states = new Dictionary<Type, VampireBatState>();
        private PlayerController _player;

        private void Start()
        {
            Initialize();
        }

        private Vector3 _prevPosition;

        private void Update()
        {
            var velocity = transform.position - _prevPosition;
            _prevPosition = transform.position;

            CurrentState.Execute(_player, this);
            DirectionController.UpdateVelocity(velocity);
        }

        // Animation Event から呼び出す。
        public void Attack()
        {
            AttackHitBox.Clear();
            var sign = DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            AttackHitBox.Fire(transform, sign, Stats.AttackPower, LayerUtility.PlayerLayerMask);
        }

        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            var damage = DefaultCalculateDamage(attackPower, Stats.Defense);

            Stats.Health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, transform.position);

            if (Stats.Health <= 0)
            {
                ChangeState<Die>();
            }
            else
            {
                ChangeState<Damage>();
            }
        }

        public void ChangeState<T>() where T : VampireBatState
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_player, this);
            }
            CurrentState = _states[typeof(T)];
            CurrentState.Enter(_player, this);

            if (!string.IsNullOrEmpty(CurrentState.AnimationName))
            {
                Animator.CrossFade(CurrentState.AnimationName, 0.1f);
            }
        }

        private void OnDrawGizmos()
        {
            if (Eye != null) Eye.DrawGizmos(transform);

            AttackHitBox.DrawGizmos(transform, 0, LayerUtility.PlayerLayerMask);
            Sensor.DrawGizmos(transform);
        }

        private void Initialize()
        {
            Stats = Instantiate(Stats);
            Eye = Instantiate(Eye);
            DirectionController.Initialize(transform);

            _player = PlayerController.Instance;
            if (_player == null) Debug.LogError("PlayerController is not found.");

            if (_idleState == null) Debug.LogError("IdleState is not found.");
            if (_flyState == null) Debug.LogError("FlyState is not found.");
            if (_approachState == null) Debug.LogError("ApproachState is not found.");
            if (_attackState == null) Debug.LogError("AttackState is not found.");
            if (_damageState == null) Debug.LogError("DamageState is not found.");
            if (_dieState == null) Debug.LogError("DieState is not found.");

            _states.Add(typeof(Idle), _idleState);
            _states.Add(typeof(Fly), _flyState);
            _states.Add(typeof(Approach), _approachState);
            _states.Add(typeof(Attack), _attackState);
            _states.Add(typeof(Damage), _damageState);
            _states.Add(typeof(Die), _dieState);

            ChangeState<Fly>();
        }
    }
}