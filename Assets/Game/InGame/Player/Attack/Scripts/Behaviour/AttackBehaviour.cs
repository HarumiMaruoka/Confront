using Confront.Debugger;
using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        private static LayerMask _layerMask;

        public static LayerMask LayerMask
        {
            get
            {
                if (_layerMask == 0)
                {
                    _layerMask = LayerMask.GetMask("Enemy");
                }
                return _layerMask;
            }
        }

        public abstract string AnimationName { get; }
        public Action<PlayerController> OnTransitionX;
        public Action<PlayerController> OnTransitionY;
        public Action<PlayerController> OnCompleted;

        public abstract void Enter(PlayerController player);

        public abstract void Execute(PlayerController player);

        public abstract void Exit(PlayerController player);


        private void OnEnable()
        {
            GizmoDrawer.OnDrawGizmosEvent += OnDrawGizmos;
        }

        private void OnDisable()
        {
            GizmoDrawer.OnDrawGizmosEvent -= OnDrawGizmos;
        }

        protected virtual void OnDrawGizmos() { }
    }

    public enum ComboInput
    {
        None,
        X,
        Y
    }
}