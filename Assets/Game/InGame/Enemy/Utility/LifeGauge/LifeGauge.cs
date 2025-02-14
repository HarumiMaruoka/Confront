using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class LifeGauge : MonoBehaviour
    {
        [SerializeField]
        private GameObject _background;
        [SerializeField]
        private GameObject _fill;

        private float _maxHealth;
        private float _maxScale;

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            _maxScale = _background.transform.localScale.x;
        }

        public void UpdateHealth(float currentHealth)
        {
            var scale = _fill.transform.localScale;
            scale.x = _maxScale * currentHealth / _maxHealth;
            _fill.transform.localScale = scale;
        }
    }
}