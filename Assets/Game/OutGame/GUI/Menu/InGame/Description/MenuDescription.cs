using System;
using UnityEngine;

namespace Confront.GUI
{
    // メニュー画面の説明を表示するクラス。
    public class MenuDescription : MonoBehaviour
    {
        [SerializeField]
        private SpriteCrossfader _backgroundView;
        [SerializeField]
        private SmoothImageSwitcher _iconView;
        [SerializeField]
        private SmoothTextSwitcher _titleView;
        [SerializeField]
        private SmoothTextSwitcher _descriptionView;

        private void Awake()
        {
            MenuDescriptionSender.OnDescriptionUpdate += Receive;
        }

        private void Receive(Sprite background, Sprite icon, string title, string description)
        {
            if (_backgroundView) _backgroundView.SetSprite(background);
            if (_iconView) _iconView.SetSprite(icon);
            if (_titleView) _titleView.SetText(title);
            if (_descriptionView) _descriptionView.SetText(description);
        }
    }
}