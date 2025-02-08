using Confront.AttackUtility;
using Confront.Enemy.Slimey;
using Confront.GameUI;
using Confront.Player;
using Confront.Utility;
using NexEditor;
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
        [Expandable] public SlimeyStats Stats;
        [Expandable] public EnemyEye Eye;
        public DirectionController DirectionController;

        [Header("States")]
        [Expandable] public Idle Idle;
        [Expandable] public Wander Wander;
        [Expandable] public Approach Approach;
        [Expandable] public Attack Attack;
        [Expandable] public Block Block;
        [Expandable] public Damage Damage;
        [Expandable] public Dead Dead;

        [Header("Attack")]
        public HitBoxOneFrame HitBox;

        [Header("Velocity")]
        public Vector2 Velocity = Vector2.zero;
        public float Gravity = -9.8f;

        [Header("Damage")]
        public float DamgageTransitionProbability = 0.5f; // ダメージを受けたときにひるむ（DamageStateに遷移する）確率

        [SerializeField, HideInInspector]
        private BullvarState _currentState = null;

        private Dictionary<Type, BullvarState> _states = new Dictionary<Type, BullvarState>();

        private void Start()
        {
            Initialized();
            ChangeState<Idle>();
        }

        private void Initialized()
        {
            _states.Add(typeof(Idle), Instantiate(Idle));
            _states.Add(typeof(Wander), Instantiate(Wander));
            _states.Add(typeof(Approach), Instantiate(Approach));
            _states.Add(typeof(Attack), Instantiate(Attack));
            _states.Add(typeof(Block), Instantiate(Block));
            _states.Add(typeof(Damage), Instantiate(Damage));
            _states.Add(typeof(Dead), Instantiate(Dead));

            DirectionController.Initialize(transform);
        }

        private void Update()
        {
            _currentState?.Execute(PlayerController.Instance, this);
            DirectionController.UpdateDirection(Velocity);
            CharacterController.Move(Velocity * Time.deltaTime);

            if (CharacterController.isGrounded) Velocity.y = 0;
            else Velocity.y += Gravity * Time.deltaTime;
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

        public void ProccesAttack() // アニメーションイベントから呼び出す
        {
            HitBox.Fire(
                transform,
                DirectionController.CurrentDirection == Direction.Right ? 1 : -1, Stats.AttackPower,
                LayerUtility.PlayerLayerMask);
        }

        private void OnDrawGizmos()
        {
            if (Eye) Eye.DrawGizmos(transform);
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

        public void TakeDamage(float attackPower, Vector2 damageVector)
        {
            var damage = DefaultCalculateDamage(attackPower, Stats.Defense);

            Stats.Health -= damage;
            DamageDisplaySystem.Instance.ShowDamage((int)damage, transform.position);

            if (Stats.Health <= 0)
            {
                ChangeState<Dead>();
            }
            else if (_currentState is Block)
            {
                // Do nothing.
            }
            else if (UnityEngine.Random.value < DamgageTransitionProbability)
            {
                ChangeState<Damage>();
            }

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