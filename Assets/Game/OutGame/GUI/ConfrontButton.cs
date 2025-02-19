using Confront.Audio;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.GUI
{
    public class ConfrontButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, ISelectHandler
    {
        public AudioClip HoverSound;
        public AudioClip ClickSound;

        private Selectable _selectable;

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            if (!_selectable) Debug.LogError("No Selectable component found on " + gameObject.name);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_selectable) _selectable.Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.PlaySE(HoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.PlaySE(ClickSound);
        }
    }
}