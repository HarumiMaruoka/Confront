using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public class LeviathanController : BossBase
    {
        public CharacterController CharacterController;
        public StateMachine StateMachine;
        public StateSelector StateSelector;

        private void Start()
        {
            StateMachine.Initialize(this);
            StateSelector.Owner = this;
            StateSelector.RefreshRegionCenters();
        }

        private void OnValidate()
        {
            if (StateSelector != null)
            {
                StateSelector.Owner = this;
                StateSelector.RefreshRegionCenters();
            }
        }

        private void Update()
        {
            // StateMachine.Update();
        }

        private void OnDrawGizmos()
        {
            if (StateSelector != null)
            {
                StateSelector.OnDrawGizmos();
            }
        }
    }
}