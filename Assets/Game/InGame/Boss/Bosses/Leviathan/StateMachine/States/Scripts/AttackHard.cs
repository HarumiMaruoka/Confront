using Confront.AttackUtility;
using Confront.Enemy;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/AttackHard")]
    public class AttackHard : TransitionableStateBase, IState
    {
        [Header("Attack")]
        [SerializeField]
        private float _attackPower = 1f;
        [SerializeField]
        private HitBox _hitBox;

        [Header("Duration")]
        [SerializeField]
        private float _duration = 1f;

        private float _elapsed = 0f;

        public string AnimationName => "AttackHard";

        public void DrawGizmos(Transform center) => _hitBox.DrawGizmos(center, _elapsed, LayerUtility.PlayerLayerMask);

        public void Initialize()
        {
            _elapsed = 0f;
        }

        public void Enter(LeviathanController owner)
        {
            _elapsed = 0f;
            _hitBox.Clear();
        }

        public void Execute(LeviathanController owner)
        {
            _elapsed += Time.deltaTime;
            _hitBox.Update(owner.transform, _attackPower, owner.DirectionSign, _elapsed, LayerUtility.PlayerLayerMask, false);
            if (_elapsed >= _duration) TransitionToNextState(owner);
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}