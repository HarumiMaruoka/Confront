using Confront.Player;
using UnityEngine;

namespace Confront.Equipment
{
    public class EquipmentItem
    {
        public EquipmentData Data { get; private set; }
        public EquipmentLevelUp LevelManager;
        public EquipmentStats Stats => Data.GetStats(LevelManager.Level);

        public EquipmentItem(EquipmentData data)
        {
            Data = data ?? throw new System.ArgumentNullException(nameof(data));
            LevelManager = new EquipmentLevelUp(CurrencyManager.Instance, PlayerController.Instance.ForgeItemInventory, Data.LevelUpRequirements, 0);
        }
    }
}