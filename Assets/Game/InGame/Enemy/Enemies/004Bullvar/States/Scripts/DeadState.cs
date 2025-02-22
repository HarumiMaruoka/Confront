﻿using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Enemy.Bullvar
{
    // 死亡時の処理
    [CreateAssetMenu(fileName = "DeadState", menuName = "ConfrontSO/Enemy/Bullvar/States/DeadState")]
    public class DeadState : BullvarState
    {
        [SerializeField]
        private string _animationName;

        public float Deceleration = 8f;
        public float Duration = 3f;

        private float _timer;

        public override string AnimationName => _animationName;

        public override void Enter(PlayerController player, BullvarController bullvar)
        {
            _timer = 0;
            bullvar.DropItem(bullvar.transform.position).Forget();
        }

        public override void Execute(PlayerController player, BullvarController bullvar)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                bullvar.gameObject.SetActive(false);
            }

            bullvar.Velocity.x = Mathf.Lerp(bullvar.Velocity.x, 0, Deceleration * Time.deltaTime);
        }

        public override void Exit(PlayerController player, BullvarController bullvar)
        {

        }
    }
}