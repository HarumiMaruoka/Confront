using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.ForgeItem
{
    public class ForgeItemInventoryUIElement : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMPro.TextMeshProUGUI _count;

        public ForgeItemData ForgeItemData { set => _icon.sprite = value.Icon; }
        public int Count { set => _count.text = value.ToString(); }

        internal void UpdateView(ForgeItemData data, int count)
        {
            // アイコンでこのアイテムの所持数が更新されたかを確認する。
            if (data?.Icon != _icon.sprite) return;
            _count.text = count.ToString();
        }
    }
}