using Confront.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Item
{
    public class HotBarIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI _count;


        [SerializeField]
        private Direction _direction;

        private InventorySlot _slot;

        private void Start()
        {
            var player = PlayerController.Instance;
            if (player == null)
            {
                Debug.LogError("PlayerController is not exist.");
                return;
            }

            _slot = player.HotBar.GetSlot(_direction);
            _slot.OnAnyChanged += UpdateUI;
            UpdateUI(_slot);
        }

        private void OnDestroy()
        {
            _slot.OnAnyChanged -= UpdateUI;
        }

        private void UpdateUI(InventorySlot slot)
        {
            if (slot == null || slot.Item == null)
            {
                _icon.sprite = null;
                _count.text = "";
                _icon.color = Color.clear;
            }
            else
            {
                _icon.sprite = slot.Item.Icon;
                _count.text = slot.Count.ToString();
                _icon.color = Color.white;
            }
        }
    }
}