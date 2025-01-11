using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Stunned")]
    public class Stunned : ScriptableObject, IState
    {
        [SerializeField]
        private float _duration = 1f;

        private float _timer = 0f;

        public string AnimationName => "Stunned";

        public void Enter(LeviathanController owner)
        {

        }

        public void Execute(LeviathanController owner)
        {

        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}
