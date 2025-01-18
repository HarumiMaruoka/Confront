using Confront.AttackUtility;
using Confront.Enemy;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/AttackSpecial")]
    public class AttackSpecial : TransitionableStateBase, IState
    {
        [Header("Attack")]
        [SerializeField]
        private float _attackPower = 1f;
        [SerializeField]
        private HitSphere[] _hitSpheres;

        [Header("Duration")]
        [SerializeField]
        private float _duration = 1f;

        private float _elapsed = 0f;

        public string AnimationName => "AttackSpecial";

        public void DrawGizmos(Transform center)
        {
            for (int i = 0; i < _hitSpheres.Length; i++)
            {
                _hitSpheres[i].DrawGizmos(center, _elapsed, LayerUtility.PlayerLayerMask);
            }
        }

        public void Initialize()
        {
            _elapsed = 0f;
        }

        public void Enter(LeviathanController owner)
        {
            _elapsed = 0f;
            for (int i = 0; i < _hitSpheres.Length; i++)
            {
                _hitSpheres[i].Clear();
            }
        }

        public void Execute(LeviathanController owner)
        {
            _elapsed += Time.deltaTime;

            for (int i = 0; i < _hitSpheres.Length; i++)
            {
                _hitSpheres[i].Update(owner.transform, _attackPower, owner.DirectionSign, _elapsed, LayerUtility.PlayerLayerMask);
            }

            if (_elapsed >= _duration)
            {
                TransitionToNextState(owner);
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}