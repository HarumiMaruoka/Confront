﻿using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "Attack", menuName = "ConfrontSO/Enemy/VampireBat/States/Attack")]
    public class Attack : VampireBatState
    {
        public float AttackInterval = 1.0f;

        private float _attackTimer;

        public override string AnimationName => "Attack";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {
            _attackTimer = -1f;
        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            if (!vampireBat.Eye.IsVisiblePlayer(vampireBat.transform, player, vampireBat.DirectionController.CurrentDirection))
            {
                vampireBat.ChangeState<Fly>();
                return;
            }
            if (!vampireBat.AttackHitBox.IsOverlapping(vampireBat.transform, LayerUtility.PlayerLayerMask))
            {
                vampireBat.ChangeState<Approach>();
                return;
            }

            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0f)
            {
                vampireBat.Animator.CrossFade(AnimationName, 0.1f);
                _attackTimer = AttackInterval;
            }
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }
    }
}