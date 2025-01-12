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

        private void Update()
        {
            // StateMachine.Update();
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