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
        public Rigidbody Rigidbody;
        public Animator Animator;
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
        private AnimatorPauseHandler _animatorPauseHandler;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            _animatorPauseHandler = new AnimatorPauseHandler(Animator);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _animatorPauseHandler.Dispose();
        }

        private Vector3 _prevPosition;

        private void Update()
        {
            if (MenuController.IsOpened) return;

            var velocity = transform.position - _prevPosition;
            _prevPosition = transform.position;

            CurrentState.Execute(_player, this);
            DirectionController.UpdateDirection(velocity);
        }

        // Animation Event から呼び出す。
        public void Attack()
        {
            AttackHitBox.Clear();
            var sign = DirectionController.CurrentDirection == Direction.Right ? 1 : -1;
            AttackHitBox.Fire(transform, sign, Stats.AttackPower, LayerUtility.PlayerLayerMask, false);
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
            if (Eye != null) Eye.DrawGizmos(transform, DirectionController.CurrentDirection);

            AttackHitBox.DrawGizmos(transform, 0, LayerUtility.PlayerLayerMask);
            Sensor.DrawGizmos(transform);
        }

        private void Initialize()
        {
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

            _states.Add(typeof(Idle), Instantiate(_idleState));
            _states.Add(typeof(Fly), Instantiate(_flyState));
            _states.Add(typeof(Approach), Instantiate(_approachState));
            _states.Add(typeof(Attack), Instantiate(_attackState));
            _states.Add(typeof(Damage), Instantiate(_damageState));
            _states.Add(typeof(Die), Instantiate(_dieState));

            ChangeState<Fly>();
        }

        protected override string CreateSaveData()
        {
            var saveData = new VampireBatSaveData
            {
                Health = Stats.Health,
                Position = transform.position,
                Direction = DirectionController.CurrentDirection,
                Rotation = transform.rotation
            };
            return saveData.CreateSaveData();
        }

        protected override void Load(string saveData)
        {
            var data = VampireBatSaveData.Load(saveData);
            if (data == null) return;

            if (data.Value.Health <= 0)
            {
                gameObject.SetActive(false);
            }

            Stats.Health = data.Value.Health;
            transform.position = data.Value.Position;
            DirectionController.CurrentDirection = data.Value.Direction;
            transform.rotation = data.Value.Rotation;
        }

        [Serializable]
        public struct VampireBatSaveData
        {
            public float Health;
            public Vector3 Position;
            public Direction Direction;
            public Quaternion Rotation;
            public string CreateSaveData()
            {
                return JsonUtility.ToJson(this);
            }
            public static VampireBatSaveData? Load(string saveData)
            {
                return JsonUtility.FromJson<VampireBatSaveData>(saveData);
            }
        }

        protected override void Reset()
        {
            base.Reset();
            ChangeState<Fly>();
        }
    }
}