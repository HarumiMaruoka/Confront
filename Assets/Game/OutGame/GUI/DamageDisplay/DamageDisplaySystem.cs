using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.GUI
{
    [DefaultExecutionOrder(-100)]
    public class DamageDisplaySystem : MonoBehaviour
    {
        public static DamageDisplaySystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Debug.LogError("DamageDisplaySystem is already exist.");
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [SerializeField]
        private Transform _container;
        [SerializeField]
        private DamageDisplay _damageDisplayPrefab;

        private HashSet<DamageDisplay> _actives = new HashSet<DamageDisplay>();
        private Stack<DamageDisplay> _inactives = new Stack<DamageDisplay>();

        public void ShowDamage(int damage, Vector3 worldPosition)
        {
            DamageDisplay display = GetDisplay();
            var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            display.transform.position = screenPosition;
            display.Show(damage);

            display.OnHide -= HideDisplay; // 重複登録を防ぐ
            display.OnHide += HideDisplay;
        }

        private DamageDisplay GetDisplay()
        {
            DamageDisplay display;
            if (_inactives.Count > 0)
            {
                display = _inactives.Pop();
            }
            else
            {
                display = Instantiate(_damageDisplayPrefab, _container);
            }
            display.gameObject.SetActive(true);
            _actives.Add(display);
            return display;
        }

        public void HideDisplay(DamageDisplay display)
        {
            display.gameObject.SetActive(false);
            _actives.Remove(display);
            _inactives.Push(display);
        }
    }
}