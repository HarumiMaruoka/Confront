using Confront.Player;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.Weapon
{
    public class InventoryWindowElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        public Button Button;

        [SerializeField]
        private Image _icon;
        [SerializeField]
        private Image _isEquipped;

        public event Action<WeaponInstance> OnClick;
        public event Action<WeaponInstance> OnMouseEnter;

        private WeaponInstance _weapon;
        public WeaponInstance Weapon
        {
            get => _weapon;
            set
            {
                _weapon = value;
                _icon.sprite = _weapon.Data.Icon;

                if (PlayerController.Instance)
                {
                    _isEquipped.gameObject.SetActive(PlayerController.Instance.EquippedWeapon == _weapon);
                }
            }
        }

        private void OnEnable()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.OnWeaponEquipped += OnWeaponEquipped;
            }
        }

        private void OnWeaponEquipped(WeaponInstance instance)
        {
            _isEquipped.gameObject.SetActive(instance == _weapon);
        }

        private void OnDisable()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.OnWeaponEquipped -= OnWeaponEquipped;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(_weapon);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnter?.Invoke(_weapon);
        }
    }
}