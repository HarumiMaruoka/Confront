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
        [Expandable] public SlimeyStats Stats;
        public EnemyEye Eye;

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

        [Header("Utility")]
        public HitBoxOneFrame AttackHitBox;
        public DirectionController DirectionController;

        [Header("For Checking (Do not modify from the editor)")]
        public SlimeyState CurrentState;

        private Dictionary<Type, SlimeyState> _states = new Dictionary<Type, SlimeyState>();
        private PlayerController _player;
        private RigidbodyPauseHandler _rigidbodyPauseHandler;
        private AnimatorPauseHandler _animatorPauseHandler;

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

        private void Update()
        {
            if (MenuController.IsOpened) return;

            CurrentState.Execute(_player, this);
            DirectionController.UpdateVelocity(Rigidbody.velocity);
        }

        // Animation Event から呼び出す。
        public virtual void Attack()
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
                ChangeState<DeadState>();
            }
            else
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
                Animator.CrossFade(CurrentState.AnimationName, 0.5f);
            }
        }

        private void OnDrawGizmos()
        {
            if (Eye != null) Eye.DrawGizmos(transform);

            AttackHitBox.DrawGizmos(transform, 0, LayerUtility.PlayerLayerMask);
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
        }

        [Serializable]
        public struct SlimeySaveData
        {
            public float Health;
            public Vector3 Position;
            public Direction Direction;
            public Quaternion Rotation;

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