using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    // 敵のダメージを受けた状態を表すステートです。
    [CreateAssetMenu(fileName = "DamageState", menuName = "ConfrontSO/Enemy/Slimey/States/DamageState")]
    public class DamageState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "GetHit";

        public float Duration = 0.5f;

        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timer = 0;
        }
        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                slimey.ChangeState<WanderState>();
            }
        }
        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}