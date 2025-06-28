using Confront.ActionItem;
using Confront.Player;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class SlotUI : MonoBehaviour, ISelectHandler
    {
        public Button Button;

        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI _countText;

        private InventorySlot _slot;
        public InventorySlot Slot
        {
            get => _slot;
            set
            {
                if (_slot != null) _slot.OnAnyChanged -= UpdateUI;
                _slot = value;
                if (_slot != null) _slot.OnAnyChanged += UpdateUI;
                UpdateUI(_slot);
            }
        }

        public event Action<SlotUI> OnSlotSelected;

        public void OnSelect(BaseEventData eventData)
        {
            OnSlotSelected?.Invoke(this);
        }

        public void UpdateUI(InventorySlot slot)
        {
            if (slot == null || slot.Item == null)
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
                _countText.text = "";
            }
            else
            {
                _icon.sprite = slot.Item.Icon;
                _icon.color = Color.white;
                _countText.text = slot.Count.ToString();
            }
        }
    }
}