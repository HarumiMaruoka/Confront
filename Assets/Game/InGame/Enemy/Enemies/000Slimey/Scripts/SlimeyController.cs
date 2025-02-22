using Confront.AttackUtility;
using Confront.Enemy.Slimey;
using Confront.GameUI;
using Confront.Player;
using Confront.Utility;
using NexEditor;
using OdinSerializer;
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
        [Expandable] public EnemyEye Eye;
        public DirectionController DirectionController;

        [Header("States")]
        [SerializeField, Expandable]
        private IdleState _idleState;
        [SerializeField, Expandable]
        private WanderState _wanderState;
        [SerializeField, Expandable]
        private ApproachState _approachState;
        [SerializeField, Expandable]
        private AttackState _attackState;
        [SerializeField, Expandable]
        private DamageState _damageState;
        [SerializeField, Expandable]
        private DeadState _deadState;

        [Header("Spawn Points")]
        [SerializeField]
        private Transform[] _spawnPoints;

        [Header("Damage")]
        public float DamgageTransitionProbability = 0.5f; // ダメージを受けたときにひるむ（DamageStateに遷移する）確率

        [Header("For Checking (Do not modify from the editor)")]
        public SlimeyState CurrentState;

        private Dictionary<Type, SlimeyState> _states = new Dictionary<Type, SlimeyState>();
        private PlayerController _player;
        private RigidbodyPauseHandler _rigidbodyPauseHandler;
        private AnimatorPauseHandler _animatorPauseHandler;

        public GameObject Owner => gameObject; // IDamageable

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            _rigidbodyPauseHandler = new RigidbodyPauseHandler(Rigidbody);
            _animatorPauseHandler = new AnimatorPauseHandler(Animator);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _rigidbodyPauseHandler.Dispose();
            _animatorPauseHandler.Dispose();
        }

        protected virtual void Update()
        {
            if (MenuController.IsOpened) return;

            CurrentState.Execute(_player, this);
            DirectionController.UpdateDirection(Rigidbody.velocity);
        }

        public void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point)
        {
            var damage = DefaultCalculateDamage(attackPower, Stats.Defense);

            Stats.Health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, transform.position);

            if (Stats.Health <= 0)
            {
                if (CurrentState is DeadState) return;
                ChangeState<DeadState>();
            }
            else if (CurrentState is BlockState)
            {
                // Do nothing.
            }
            else if (UnityEngine.Random.value < DamgageTransitionProbability)
            {
                ChangeState<DamageState>();
            }
        }

        public void ChangeState<T>() where T : SlimeyState
        {
            ChangeState(typeof(T));
        }

        public void ChangeState(Type type)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit(_player, this);
            }
            CurrentState = _states[type];
            CurrentState.Enter(_player, this);
            if (!string.IsNullOrEmpty(CurrentState.AnimationName))
            {
                Animator.CrossFade(CurrentState.AnimationName, 0.1f);
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (Eye != null) Eye.DrawGizmos(transform, DirectionController.CurrentDirection);

            if (CurrentState) CurrentState.DrawGizmos(_player, this);
            if (_idleState) _idleState.DrawGizmos(_player, this);
            if (_wanderState) _wanderState.DrawGizmos(_player, this);
            if (_approachState) _approachState.DrawGizmos(_player, this);
            if (_attackState) _attackState.DrawGizmos(_player, this);
            if (_damageState) _damageState.DrawGizmos(_player, this);
            if (_deadState) _deadState.DrawGizmos(_player, this);
        }

        private void Initialize()
        {
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

            _states.Add(typeof(IdleState), Instantiate(_idleState));
            _states.Add(typeof(WanderState), Instantiate(_wanderState));
            _states.Add(typeof(ApproachState), Instantiate(_approachState));
            _states.Add(typeof(AttackState), Instantiate(_attackState));
            _states.Add(typeof(DamageState), Instantiate(_damageState));
            _states.Add(typeof(DeadState), Instantiate(_deadState));

            ChangeState<IdleState>();
        }

        protected override string CreateSaveData()
        {
            var saveData = new SlimeySaveData
            {
                Health = Stats.Health,
                Position = transform.position,
                Direction = DirectionController.CurrentDirection,
                Rotation = DirectionController.CurrentRotation,
                Velocity = Rigidbody.velocity,
                AngularVelocity = Rigidbody.angularVelocity
            };
            return saveData.CreateSaveData();
        }

        protected override void Load(string saveData)
        {
            var data = SlimeySaveData.Load(saveData);
            if (data == null) return;

            if (data.Value.Health < 0f)
            {
                this.gameObject.SetActive(false);
                return;
            }

            Stats.Health = data.Value.Health;
            transform.position = data.Value.Position;
            DirectionController.CurrentDirection = data.Value.Direction;
            transform.rotation = DirectionController.CurrentRotation;
            Rigidbody.velocity = data.Value.Velocity;
            Rigidbody.angularVelocity = data.Value.AngularVelocity;
        }

        public Vector3 GetSpawnPoint(int spawnPointIndex)
        {
            return _spawnPoints[spawnPointIndex].position;
        }

        protected override void Reset()
        {
            base.Reset();
            ChangeState<IdleState>();
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }

        [Serializable]
        public struct SlimeySaveData
        {
            public float Health;
            public Vector3 Position;
            public Direction Direction;
            public Quaternion Rotation;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;

            public string CreateSaveData()
            {
                var bytes = SerializationUtility.SerializeValue(this, DataFormat.Binary);
                return Convert.ToBase64String(bytes);
            }

            public static SlimeySaveData? Load(string saveData)
            {
                if (string.IsNullOrEmpty(saveData)) return null;

                var bytes = Convert.FromBase64String(saveData);
                return SerializationUtility.DeserializeValue<SlimeySaveData>(bytes, DataFormat.Binary);
            }
        }
    }
}