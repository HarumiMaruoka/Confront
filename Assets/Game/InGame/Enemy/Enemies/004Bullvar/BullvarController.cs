using Confront.AttackUtility;
using Confront.GUI;
using Confront.Player;
using Confront.Utility;
using NexEditor;
using OdinSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    public class BullvarController : EnemyBase, IDamageable
    {
        [Header("Components")]
        public Animator Animator;
        public CharacterController CharacterController;
        [Expandable] public EnemyEye Eye;
        public DirectionController DirectionController;

        [Header("States")]
        [Expandable] public IdleState Idle;
        [Expandable] public WanderState Wander;
        [Expandable] public ApproachState Approach;
        [Expandable] public AttackState Attack;
        [Expandable] public BlockState Block;
        [Expandable] public DamageState Damage;
        [Expandable] public DeadState Dead;

        [Header("Velocity")]
        public Vector2 Velocity = Vector2.zero;
        public float Gravity = -9.8f;

        [Header("Damage")]
        public float DamgageTransitionProbability = 0.5f; // ダメージを受けたときにひるむ（DamageStateに遷移する）確率

        [Header("Attack")]
        public Transform[] ProjectileSpawnPoints;

        [SerializeField, HideInInspector]
        private BullvarState _currentState = null;

        private Dictionary<Type, BullvarState> _states = new Dictionary<Type, BullvarState>();
        private AnimatorPauseHandler _animatorPauseHandler;

        public GameObject Owner => gameObject; // IDamageable

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

        private void Initialize()
        {
            Eye = Instantiate(Eye);

            _states.Add(typeof(IdleState), Instantiate(Idle));
            _states.Add(typeof(WanderState), Instantiate(Wander));
            _states.Add(typeof(ApproachState), Instantiate(Approach));
            _states.Add(typeof(AttackState), Instantiate(Attack));
            _states.Add(typeof(BlockState), Instantiate(Block));
            _states.Add(typeof(DamageState), Instantiate(Damage));
            _states.Add(typeof(DeadState), Instantiate(Dead));

            DirectionController.Initialize(transform);

            ChangeState<IdleState>();
        }

        private void Update()
        {
            if (MenuController.IsOpened) return;

            _currentState?.Execute(PlayerController.Instance, this);
            DirectionController.UpdateDirection(Velocity);
            CharacterController.Move(Velocity * Time.deltaTime);
            CharacterController.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            CharacterController.enabled = true;

            if (CharacterController.isGrounded) Velocity.y = 0;
            else Velocity.y += Gravity * Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            if (Eye) Eye.DrawGizmos(transform, DirectionController.CurrentDirection);

            var player = PlayerController.Instance;
            if (_currentState) _currentState.DrawGizmos(player, this);
            if (Idle) Idle.DrawGizmos(player, this);
            if (Wander) Wander.DrawGizmos(player, this);
            if (Approach) Approach.DrawGizmos(player, this);
            if (Attack) Attack.DrawGizmos(player, this);
            if (Block) Block.DrawGizmos(player, this);
            if (Damage) Damage.DrawGizmos(player, this);
            if (Dead) Dead.DrawGizmos(player, this);
        }

        public Vector3? GetSpawnPoint(int spawnPointIndex)
        {
            if (ProjectileSpawnPoints == null || ProjectileSpawnPoints.Length == 0)
            {
                Debug.LogError("ProjectileSpawnPoints is empty.");
                return null;
            }
            if (spawnPointIndex < 0 || spawnPointIndex >= ProjectileSpawnPoints.Length)
            {
                Debug.LogError("Invalid spawnPointIndex.");
                return null;
            }
            return ProjectileSpawnPoints[spawnPointIndex].position;
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
            Animator.CrossFade(bullvarState.AnimationName, 0.1f);
            _currentState.Enter(PlayerController.Instance, this);
        }

        private void OnAnimatorMove()
        {
            // Do nothing.
        }

        protected override string CreateSaveData()
        {
            var saveData = new BullvarSaveData
            {
                Health = Stats.Health,
                Position = transform.position,
                Direction = DirectionController.CurrentDirection,
                Rotation = DirectionController.CurrentRotation,
                Velocity = Velocity,
            };
            return saveData.CreateSaveData();
        }

        protected override void Load(string saveData)
        {
            var data = BullvarSaveData.Load(saveData);
            if (data == null) return;

            if (data.Value.Health < 0f)
            {
                this.gameObject.SetActive(false);
                return;
            }

            Stats.Health = data.Value.Health;
            CharacterController.enabled = false;
            transform.position = data.Value.Position;
            CharacterController.enabled = true;
            DirectionController.CurrentDirection = data.Value.Direction;
            transform.rotation = DirectionController.CurrentRotation;
            Velocity = data.Value.Velocity;
        }

        public void TakeDamage(float attackPower, Vector2 damageVector, Vector3 point)
        {
            var damage = DefaultCalculateDamage(attackPower, Stats.Defense);

            Stats.Health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, point);

            if (Stats.Health <= 0)
            {
                if (_currentState is DeadState) return;
                ChangeState<DeadState>();
            }
            else if (_currentState is AttackState or BlockState)
            {
                // Do nothing.
            }
            else if (UnityEngine.Random.value < DamgageTransitionProbability)
            {
                ChangeState<DamageState>();
            }
            else if (_currentState is not ApproachState)
            {
                ChangeState<ApproachState>();
            }
        }

        protected override void Reset()
        {
            base.Reset();
            ChangeState<IdleState>();
            Velocity = Vector2.zero;
        }
    }

    [Serializable]
    public struct BullvarSaveData
    {
        public float Health;
        public Vector3 Position;
        public Direction Direction;
        public Quaternion Rotation;
        public Vector2 Velocity;

        public string CreateSaveData()
        {
            var bytes = SerializationUtility.SerializeValue(this, DataFormat.Binary);
            return Convert.ToBase64String(bytes);
        }

        public static BullvarSaveData? Load(string saveData)
        {
            if (string.IsNullOrEmpty(saveData)) return null;

            var bytes = Convert.FromBase64String(saveData);
            return SerializationUtility.DeserializeValue<BullvarSaveData>(bytes, DataFormat.Binary);
        }
    }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BullvarController))]
    public class BullvarControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 現在のステートを表示
            var currentState = serializedObject.FindProperty("_currentState");

            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            UnityEditor.EditorGUILayout.ObjectField("Current State", currentState.objectReferenceValue, typeof(BullvarState), false);
            UnityEditor.EditorGUI.EndDisabledGroup();
        }
    }
#endif
}