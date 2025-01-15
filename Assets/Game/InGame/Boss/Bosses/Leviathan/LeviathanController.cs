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

        public TMPro.TextMeshProUGUI DebugText;

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
        }

        private void OnDrawGizmos()
        {
            if (AttackStateSelector != null)
            {
                AttackStateSelector.OnDrawGizmos();
            }
        }
    }
}