using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Confront.GameUI
{
    public class UISelectionHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _defaultSelection;

        private GameObject _previouseSelection;

        private void OnEnable()
        {
            _previouseSelection = EventSystem.current?.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(_defaultSelection);
        }

        private void OnDisable()
        {
            if (_previouseSelection)
            {
                EventSystem.current?.SetSelectedGameObject(_previouseSelection);
            }
            else
            {
                Debug.LogWarning("Previous selection is null");
            }
        }
    }
}