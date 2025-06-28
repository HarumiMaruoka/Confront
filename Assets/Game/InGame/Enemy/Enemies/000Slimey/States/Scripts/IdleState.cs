using System;
using UnityEngine;
using Confront.Player;

namespace Confront.Enemy.Slimey
{
    // 	スライムがその場で動かず、待機している状態。特に敵やプレイヤーが近づいていない時に見せる行動です。
    [CreateAssetMenu(fileName = "IdleState", menuName = "ConfrontSO/Enemy/Slimey/States/IdleState")]
    public class IdleState : SlimeyState
    {
        [SerializeField]
        private string _animationName = "IdleNormal";

        public float MinDuration = 1f;
        public float MaxDuration = 2f;

        [Header("速度があった場合、停止する処理に使用される減速度")]
        public float Deceleration = 10f;

        private float _duration;
        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _duration = UnityEngine.Random.Range(MinDuration, MaxDuration);
            _timer = 0;
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            if (slimey.Eye.IsVisiblePlayer(slimey.transform, player, slimey.DirectionController.CurrentDirection))
            {
                slimey.ChangeState<ApproachState>();
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                slimey.ChangeState<WanderState>();
                return;
            }

            var xSpeed = Mathf.MoveTowards(slimey.Rigidbody.linearVelocity.x, 0, Deceleration * Time.deltaTime);
            slimey.Rigidbody.linearVelocity = new Vector2(xSpeed, slimey.Rigidbody.linearVelocity.y);
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}