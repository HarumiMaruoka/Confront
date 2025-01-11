using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "Damage", menuName = "ConfrontSO/Enemy/VampireBat/States/Damage")]
    public class Damage : VampireBatState
    {
        public float Duration = 0.5f;

        private float _timer;

        public override string AnimationName => "Damage";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {
            _timer = 0;
        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                vampireBat.ChangeState<Fly>();
            }
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }
    }
}