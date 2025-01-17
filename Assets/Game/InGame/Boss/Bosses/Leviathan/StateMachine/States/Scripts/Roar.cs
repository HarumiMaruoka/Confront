using Confront.AttackUtility;
using Confront.Enemy;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Roar")]
    public class Roar : TransitionableStateBase, IState
    {
        [Header("Attack")]
        [SerializeField]
        private float _attackPower = 1f;
        [SerializeField]
        private HitSphere _hitSphere;

        [Header("Duration")]
        [SerializeField]
        private float _duration = 1f;

        private float _elapsed = 0f;

        public string AnimationName => "Roar";

        public void DrawGizmos(Transform center) => _hitSphere.DrawGizmos(center, _elapsed, EnemyBase.PlayerLayerMask);

        public void Initialize()
        {
            _elapsed = 0f;
        }

        public void Enter(LeviathanController owner)
        {
            _elapsed = 0f;
            _hitSphere.Clear();
        }

        public void Execute(LeviathanController owner)
        {
            _elapsed += Time.deltaTime;
            var directionSign = PlayerController.Instance.transform.position.x - owner.transform.position.x > 0 ? 1 : -1;
            _hitSphere.Update(owner.transform, _attackPower, directionSign, _elapsed, EnemyBase.PlayerLayerMask);
            if (_elapsed >= _duration) TransitionToNextState(owner);
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}
