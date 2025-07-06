using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Equipment.UI
{
    public class EquipmentStatCompareView : MonoBehaviour
    {
        private void UpdateView(EquipmentItem compareItem)
        {
            if (compareItem == null)
            {
                ResetView();
                return;
            }

            EquipmentItem equippedItem = PlayerController.Instance.Equipped.GetEquipment(compareItem.Data.EquipmentType);

            var equippedStats = equippedItem != null ? equippedItem.Stats : default;
            var compareStats = compareItem.Stats;
        }

        private void ResetView()
        {

        }
    }
}