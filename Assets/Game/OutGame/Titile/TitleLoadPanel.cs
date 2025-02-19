using Confront.GameUI;
using System;
using UnityEngine;

namespace Confront.Title
{
    public class TitleLoadPanel : MonoBehaviour
    {
        [SerializeField]
        private ScrollToSelected scrollToSelected;

        private void Update()
        {
            var selected = UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject?.GetComponent<RectTransform>();
            if (selected) scrollToSelected.ScrollTo(selected);
        }
    }
}