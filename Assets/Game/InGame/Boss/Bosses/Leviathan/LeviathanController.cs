using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public class LeviathanController : BossBase
    {
        public Animator Animator;
        public StateMachine StateMachine;
        public AttackStateSelector AttackStateSelector;
        [NonSerialized]
        public Direction Direction = Direction.Left;

        [SerializeField]
        private StateType TestStateType;

        public TMPro.TextMeshProUGUI DebugText;

        public float DirectionSign => Direction == Direction.Right ? 1 : -1;

        private void Start()
        {
            StateMachine.Initialize(this);
            AttackStateSelector.Owner = this;
            AttackStateSelector.RefreshRegionCenters();
        }

        private void OnValidate()
        {
            if (AttackStateSelector != null)
            {
                AttackStateSelector.Owner = this;
                AttackStateSelector.RefreshRegionCenters();
            }
        }

        protected override void Update()
        {
            base.Update();
            StateMachine.Update();
            DebugText.text = StateMachine.CurrentState.GetType().Name;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestStateType.ChangeState(this);
            }
        }

        private void OnDrawGizmos()
        {
            if (AttackStateSelector != null) AttackStateSelector.OnDrawGizmos();
            if (StateMachine.Attack1) StateMachine.Attack1.DrawGizmos(transform);
            if (StateMachine.Attack2) StateMachine.Attack2.DrawGizmos(transform);
            if (StateMachine.AttackHard) StateMachine.AttackHard.DrawGizmos(transform);
            if (StateMachine.AttackSpecial) StateMachine.AttackSpecial.DrawGizmos(transform);
        }
    }
}