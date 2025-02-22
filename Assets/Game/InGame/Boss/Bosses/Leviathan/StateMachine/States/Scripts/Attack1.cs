using Confront.AttackUtility;
using Confront.Audio;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Attack1")]
    public class Attack1 : TransitionableStateBase, IState
    {
        [Header("Attack")]
        [SerializeField]
        private float _attackPower = 1f;
        [SerializeField]
        private HitBox _hitBox;

        [Header("SFX")]
        [SerializeField]
        private AudioClip _sfx;
        [SerializeField]
        private float _sfxOffset = 0.5f;

        [Header("Duration")]
        [SerializeField]
        private float _duration = 1f;

        private float _elapsed = 0f;

        public string AnimationName => "Attack1";

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
            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;
            _hitBox.Update(owner.transform, _attackPower, owner.DirectionSign, _elapsed, LayerUtility.PlayerLayerMask, false);
            if (previousElapsed <= _sfxOffset && _sfxOffset < _elapsed)
            {
                AudioManager.PlaySE(_sfx, owner.transform.position);
            }
            if (_elapsed >= _duration) TransitionToNextState(owner);
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}