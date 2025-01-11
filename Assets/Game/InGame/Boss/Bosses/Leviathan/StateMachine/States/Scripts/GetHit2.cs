using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/GetHit2")]
    public class GetHit2 : ScriptableObject, IState
    {
        [SerializeField]
        private float _duration = 1f;

        private float _timer = 0f;

        public string AnimationName => "GetHit2";

        public void Enter(LeviathanController owner)
        {
            _timer = 0f;
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                // owner.StateMachine.ChangeState(owner.Stunned);
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}
