using Confront.ActionItem;
using System;
using UnityEngine;

namespace Confront.Player
{
    public class DrinkPotion : IState
    {
        [SerializeField]
        private float _drinkTiming = 0.5f;
        [SerializeField]
        private float _duration = 1f;
        [SerializeField]
        private float _deceleration = 12f;

        private Potion _potion;
        private InventorySlot _slot;

        private float _elapsed;

        public string AnimationName => "DrinkPotion";

        public void Initialize(Potion potion, InventorySlot slot)
        {
            _potion = potion;
            _slot = slot;
        }

        public void Enter(PlayerController player)
        {
            _elapsed = 0f;
        }

        public void Execute(PlayerController player)
        {
            var previousElapsed = _elapsed;
            _elapsed += Time.deltaTime;

            Grounded.Move(player, false);

            if (previousElapsed < _drinkTiming && _elapsed >= _drinkTiming)
            {
                _potion.ApplyEffect(player, _slot);
            }
            if (_elapsed >= _duration)
            {
                this.TransitionToDefaultState(player);
            }
        }

        public void Exit(PlayerController player)
        {
            _potion = null;
        }
    }
}