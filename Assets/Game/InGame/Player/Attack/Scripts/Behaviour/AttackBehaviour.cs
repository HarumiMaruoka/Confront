﻿using Confront.Debugger;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Player.Combo
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        public static LayerMask LayerMask => LayerUtility.EnemyLayerMask | LayerUtility.NoCollisionEnemy | LayerUtility.PlatformEnemy;

        public abstract string AnimationName { get; }
        public abstract float AnimationOffset { get; }
        public Action<PlayerController> OnTransitionX;
        public Action<PlayerController> OnTransitionY;
        public Action<PlayerController> OnCompleted;

        public AudioClip[] HitSFX;

        public abstract void Enter(PlayerController player, ComboTree tree);

        public abstract void Execute(PlayerController player, ComboTree tree);

        public abstract void Exit(PlayerController player, ComboTree tree);


        protected virtual void OnEnable()
        {
            GizmoDrawer.OnDrawGizmosEvent += OnDrawGizmos;
        }

        protected virtual void OnDisable()
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