using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Rotate")]
    public class Rotate : ScriptableObject, IState
    {
        [SerializeField]
        private float _duration = 1f;

        private float _timer = 0f;

        public string AnimationName => "WalkCycle";

        public void Enter(LeviathanController owner)
        {
            _timer = 0f;
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;

            // TODO: ここに回転処理を書く

            if (_timer >= _duration)
            {
                // owner.StateMachine.ChangeState(owner.Idle);
            }
        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}