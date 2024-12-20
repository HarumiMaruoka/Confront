using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "Approach", menuName = "Enemy/VampireBat/States/Approach")]
    public class Approach : VampireBatState
    {
        public float Speed;
        public override string AnimationName => "FlyCycle";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {

        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            vampireBat.transform.position = Vector3.MoveTowards(vampireBat.transform.position, player.transform.position, Speed * Time.deltaTime);

            if (!vampireBat.Eye.IsVisiblePlayer(vampireBat.transform, player))
            {
                vampireBat.ChangeState<Fly>();
                return;
            }
            if (vampireBat.AttackHitBox.IsOverlapping(vampireBat.transform, EnemyBase.PlayerLayerMask))
            {
                vampireBat.ChangeState<Attack>();
                return;
            }
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }
    }
}