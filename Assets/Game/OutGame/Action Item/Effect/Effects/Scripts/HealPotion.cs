using Confront.Player;
using System;
using UnityEngine;

namespace Confront.ActionItem
{
    [CreateAssetMenu(menuName = "ConfrontSO/ItemEffect/HealPotion")]
    public class HealPotion : Potion
    {
        [SerializeField]
        private float _healAmount = 10f;
        [SerializeField]
        private GameObject _vfx;

        public override void ApplyEffect(PlayerController player, InventorySlot slot)
        {
            player.HealthManager.Heal(_healAmount);
            slot.Count--;
            if (_vfx) GameObject.Instantiate(_vfx, player.transform.position, Quaternion.identity);
        }

        public override void UseItem(PlayerController player, InventorySlot slot)
        {
            var state = player.StateMachine.ChangeState<DrinkPotion>();
            state.Initialize(this, slot);
        }
    }
}