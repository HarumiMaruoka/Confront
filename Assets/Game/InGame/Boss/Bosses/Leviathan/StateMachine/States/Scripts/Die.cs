using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Die")]
    public class Die : ScriptableObject, IState
    {
        [SerializeField]
        private float _duration = 1f;

        private float _timer = 0f;

        public string AnimationName => "Die";

        public void Enter(LeviathanController owner)
        {
            _timer = 0f;
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                // TODO: ownerを破棄する。
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}