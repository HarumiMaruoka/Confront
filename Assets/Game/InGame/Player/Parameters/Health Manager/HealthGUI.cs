using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Player
{
    [DefaultExecutionOrder(10)] // PlayerControllerより後に実行されるようにする
    public class HealthGUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;
        [SerializeField]
        private Slider _healthSlider;

        private void Start()
        {
            _healthSlider.interactable = false;
            _healthSlider.minValue = 0;

            _playerController.HealthManager.OnHealthChanged += OnHealthChanged;
            _playerController.HealthManager.OnMaxHealthChanged += OnMaxHealthChanged;
            OnMaxHealthChanged(_playerController.HealthManager.MaxHealth, _playerController.HealthManager.CurrentHealth);
        }

        private void OnMaxHealthChanged(float maxHealth, float currentHealth)
        {
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHealth;
        }

        private void OnHealthChanged(float currentHealth)
        {
            _healthSlider.value = currentHealth;
        }
    }
}