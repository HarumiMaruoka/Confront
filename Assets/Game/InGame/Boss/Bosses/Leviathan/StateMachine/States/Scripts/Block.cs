using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Block")]
    public class Block : TransitionableStateBase, IState
    {
        [SerializeField]
        private float _duration = 1f;
        [SerializeField]
        private float _defenseBoost = 80f;

        private float _timer = 0f;

        public string AnimationName => "Block";

        public void Enter(LeviathanController owner)
        {
            _timer = 0f;
            owner.Stats.Defense += _defenseBoost;
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                TransitionToNextState(owner);
            }
        }

        public void Exit(LeviathanController owner)
        {
            owner.Stats.Defense -= _defenseBoost;
        }
    }
}