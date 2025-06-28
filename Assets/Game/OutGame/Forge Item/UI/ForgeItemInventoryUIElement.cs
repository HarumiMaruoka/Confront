using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.ForgeItem
{
    public class ForgeItemInventoryUIElement : MonoBehaviour, ISelectHandler
    {
        public Button Button;

        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI _count;

        private ForgeItemData _forgeItemData;

        public ForgeItemData ForgeItemData
        {
            get => _forgeItemData;
            set
            {
                _forgeItemData = value;
                _icon.sprite = value.Icon;
            }
        }

        public int Count { set => _count.text = $"x{value}"; }

        public event Action<ForgeItemInventoryUIElement> OnSelected;

        public void OnSelect(BaseEventData eventData)
        {
            OnSelected?.Invoke(this);
        }

        internal void UpdateView(ForgeItemData data, int count)
        {
            // アイコンでこのアイテムの所持数が更新されたかを確認する。
            if (data?.Icon != _icon.sprite) return;
            _count.text = count.ToString();
        }
    }
}