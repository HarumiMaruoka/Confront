using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    [CreateAssetMenu(fileName = "BlockState", menuName = "ConfrontSO/Enemy/Slimey/States/BlockState")]
    public class BlockState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "Block";

        [Header("速度があった場合、停止する処理に使用される減速度")]
        public float Deceleration = 10f;

        public float DefenseBoostValue = 0.5f;

        [Header("ブロック状態を維持する時間の範囲")]
        public float MinTime = 1.0f;
        public float MaxTime = 3.0f;

        private float _timer;
        private float _time;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timer = 0;
            _time = UnityEngine.Random.Range(MinTime, MaxTime);
            slimey.Stats.Defense += DefenseBoostValue;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;

            if (_timer >= _time)
            {
                if (!slimey.Eye.IsVisiblePlayer(slimey.transform, player))
                {
                    slimey.ChangeState<IdleState>();
                }
                else
                {
                    slimey.ChangeState<ApproachState>();
                }
            }

            var xSpeed = Mathf.MoveTowards(slimey.Rigidbody.velocity.x, 0, Deceleration * Time.deltaTime);
            slimey.Rigidbody.velocity = new Vector2(xSpeed, slimey.Rigidbody.velocity.y);
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {
            slimey.Stats.Defense -= DefenseBoostValue;
        }
    }
}