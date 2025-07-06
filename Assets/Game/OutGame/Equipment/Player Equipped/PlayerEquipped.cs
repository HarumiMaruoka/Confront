using System;
using UnityEngine;

namespace Confront.Equipment
{
    public class PlayerEquipped
    {
        // プレイヤーが装備しているアイテム
        private EquipmentItem helmet, armor, boots, gloves, accessory, implant = null;

        public EquipmentItem Helmet => helmet;
        public EquipmentItem Armor => armor;
        public EquipmentItem Boots => boots;
        public EquipmentItem Gloves => gloves;
        public EquipmentItem Accessory => accessory;
        public EquipmentItem Implant => implant;

        // 装備を変更する。
        public EquipmentItem ChangeEquipment(EquipmentItem equipment)
        {
            var prev = GetEquipment(equipment.Data.EquipmentType);
            SetEquipment(equipment);
            return prev;
        }

        public void SetEquipment(EquipmentItem equipment)
        {
            switch (equipment.Data.EquipmentType)
            {
                case EquipmentType.Helmet: helmet = equipment; break;
                case EquipmentType.Armor: armor = equipment; break;
                case EquipmentType.Boots: boots = equipment; break;
                case EquipmentType.Gloves: gloves = equipment; break;
                case EquipmentType.Accessory: accessory = equipment; break;
                case EquipmentType.Implant: implant = equipment; break;
                default: throw new ArgumentOutOfRangeException(nameof(equipment.Data.EquipmentType), equipment.Data.EquipmentType, null);
            }
        }

        public EquipmentItem GetEquipment(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Helmet: return helmet;
                case EquipmentType.Armor: return armor;
                case EquipmentType.Boots: return boots;
                case EquipmentType.Gloves: return gloves;
                case EquipmentType.Accessory: return accessory;
                case EquipmentType.Implant: return implant;
                default: throw new ArgumentOutOfRangeException(nameof(equipmentType), equipmentType, null);
            }
        }
    }
}