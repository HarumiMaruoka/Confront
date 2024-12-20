using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "Idle", menuName = "Enemy/VampireBat/States/Idle")]
    public class Idle : VampireBatState
    {
        public float Duration = 1.0f;

        private float _timer = 0.0f;

        public override string AnimationName => "Idle";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {
            _timer = 0.0f;
        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            if (vampireBat.Eye.IsVisiblePlayer(vampireBat.transform, player))
            {
                vampireBat.ChangeState<Approach>();
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                vampireBat.ChangeState<Fly>();
                return;
            }
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }
    }
}