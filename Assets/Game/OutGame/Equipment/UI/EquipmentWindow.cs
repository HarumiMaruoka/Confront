using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Equipment.UI
{
    public class EquipmentWindow : MonoBehaviour
    {
        [SerializeField]
        private EquipmentSlot _slotPrefab;
        [SerializeField]
        private EquipmentSlot _hoveredSlot;
        [SerializeField]
        private GridLayoutGroup _container;
        [SerializeField]
        private DescriptionView _descriptionView;

        private List<EquipmentSlot> _slots = new List<EquipmentSlot>();

        public void Initialize(EquipmentInventory inventory)
        {
            Debug.Assert(_slotPrefab != null, "Slot prefab is not assigned in the EquipmentWindow.");
            Debug.Assert(_container != null, "Container is not assigned in the EquipmentWindow.");
            Debug.Assert(inventory != null, "Inventory is not assigned in the EquipmentWindow.");

            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = Instantiate(_slotPrefab, _container.transform);
                slot.Target = inventory[i];

                slot.OnClicked += OnSlotClicked;
                slot.OnSelected += OnSlotSelected;
                _slots.Add(slot);
            }

            inventory.OnItemAdded += OnItemAdded;
            inventory.OnItemRemoved += OnItemRemoved;
            inventory.OnItemInserted += OnItemInserted;
            inventory.OnInventoryResized += OnInventoryResized;
        }

        // Slot event handlers
        private void OnSlotClicked(EquipmentSlot slot)
        {
            // 装備アイテムを選択したときの処理
            throw new NotImplementedException();
        }

        private void OnSlotSelected(EquipmentSlot slot)
        {
            _descriptionView.SetView(slot.Target);
        }

        // Inventory event handlers
        private void OnItemAdded(int index, EquipmentItem item)
        {
            Debug.Assert(index >= 0 && index < _slots.Count, "Index out of bounds for slots list.");
            _slots[index].Target = item;
        }

        private void OnItemRemoved(int index, EquipmentItem item)
        {
            Debug.Assert(index >= 0 && index < _slots.Count, "Index out of bounds for slots list.");
            _slots[index].Target = null;
        }

        private void OnItemInserted(int index, EquipmentItem item)
        {
            throw new NotImplementedException();
        }

        private void OnInventoryResized(int newSize)
        {
            throw new NotImplementedException();
        }
    }
}