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

        public override void ApplyEffect(PlayerController player, InventorySlot slot)
        {
            player.HealthManager.Heal(_healAmount);
            slot.Count--;
            // ここでポーションの効果を表現するVFXを再生する処理を追加
        }

        public override void UseItem(PlayerController player, InventorySlot slot)
        {
            var state = player.StateMachine.ChangeState<DrinkPotion>();
            state.Initialize(this, slot);
        }
    }
}