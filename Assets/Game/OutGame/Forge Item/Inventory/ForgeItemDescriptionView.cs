using Confront.GUI;
using System;
using UnityEngine;

namespace Confront.ForgeItem
{
    public class ForgeItemDescriptionView : MonoBehaviour
    {
        [SerializeField]
        private SmoothImageSwitcher _iconView;
        [SerializeField]
        private SmoothTextSwitcher _titleView;
        [SerializeField]
        private SmoothTextSwitcher _descriptionView;

        [SerializeField]
        private Sprite _transparentIcon;

        public void UpdateView(ForgeItemData itemData)
        {
            if (itemData == null)
            {
                _iconView.SetSprite(_transparentIcon);
                _titleView.SetText(null);
                _descriptionView.SetText(null);
            }
            else
            {
                _iconView.SetSprite(itemData.Icon);
                _titleView.SetText(itemData.Name);
                _descriptionView.SetText(itemData.Description);
            }
        }

        public void Clear()
        {
            _iconView.SetSprite(_transparentIcon);
            _titleView.SetText(null);
            _descriptionView.SetText(null);
        }
    }
}