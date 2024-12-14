using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Weapon
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField]
        private InventoryWindowElement _elementPrefab;
        [SerializeField]
        private Transform _container;

        private WeaponInventory _inventory;

        private Queue<InventoryWindowElement> _inactives = new Queue<InventoryWindowElement>();
        private Dictionary<WeaponInstance, InventoryWindowElement> _elementMap = new Dictionary<WeaponInstance, InventoryWindowElement>();

        private PlayerController _player;

        private void Start()
        {
            _player = PlayerController.Instance;
            if (_player == null)
            {
                Debug.LogError("PlayerController is not found.");
                return;
            }
            _inventory = _player.WeaponInventory;
            Open(_inventory);
        }

        private void OnEnable()
        {
            if (_inventory != null)
            {
                Open(_inventory);
            }
        }

        private void OnDisable()
        {
            if (_inventory != null)
            {
                Clear();
            }
        }

        #region Core Functions
        public void Open(WeaponInventory inventory)
        {
            if (_inventory != null)
            {
                _inventory.OnWeaponAdded -= OnWeaponAdded;
                _inventory.OnWeaponRemoved -= OnWeaponRemoved;
                _inventory.OnWeaponSwapped -= OnWeaponSwapped;
            }
            _inventory = inventory;
            if (_inventory != null)
            {
                _inventory.OnWeaponAdded += OnWeaponAdded;
                _inventory.OnWeaponRemoved += OnWeaponRemoved;
                _inventory.OnWeaponSwapped += OnWeaponSwapped;
            }

            Clear();
            foreach (var weapon in _inventory)
            {
                GetOrCreateElementUI(weapon);
            }
        }

        private void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.OnWeaponAdded -= OnWeaponAdded;
                _inventory.OnWeaponRemoved -= OnWeaponRemoved;
                _inventory.OnWeaponSwapped -= OnWeaponSwapped;
            }
        }

        private void Clear()
        {
            foreach (var element in _elementMap.Values)
            {
                element.gameObject.SetActive(false);
                _inactives.Enqueue(element);
            }
            _elementMap.Clear();
        }
        #endregion

        #region Inventory Events
        private InventoryWindowElement GetOrCreateElementUI(WeaponInstance weapon)
        {
            InventoryWindowElement element;
            if (_inactives.Count > 0)
            {
                element = _inactives.Dequeue();
                element.gameObject.SetActive(true);
                element.transform.SetAsLastSibling();
            }
            else
            {
                element = Instantiate(_elementPrefab, _container);
            }
            element.Weapon = weapon;
            _elementMap[weapon] = element;

            element.OnClick += OnElementClickedBuffer;
            element.OnMouseEnter += OnElementMouseEnterBuffer;
            return element;
        }

        private void RemoveElement(WeaponInstance weapon)
        {
            if (_elementMap.TryGetValue(weapon, out var element))
            {
                element.gameObject.SetActive(false);
                _inactives.Enqueue(element);
                _elementMap.Remove(weapon);

                element.OnClick -= OnElementClickedBuffer;
                element.OnMouseEnter -= OnElementMouseEnterBuffer;
            }
        }

        private void OnWeaponAdded(WeaponInstance weapon)
        {
            GetOrCreateElementUI(weapon);
        }

        private void OnWeaponSwapped(WeaponInstance a, WeaponInstance b)
        {
            // 二つのヒエラルキーの順番を入れ替える
            var elementA = _elementMap[a];
            var elementB = _elementMap[b];

            var indexA = elementA.transform.GetSiblingIndex();
            var indexB = elementB.transform.GetSiblingIndex();

            elementA.transform.SetSiblingIndex(indexB);
            elementB.transform.SetSiblingIndex(indexA);
        }

        private void OnWeaponRemoved(WeaponInstance weapon)
        {
            RemoveElement(weapon);
        }
        #endregion

        #region GUI Events
        private event Action<WeaponInstance> OnElementClickedBuffer;
        public event Action<WeaponInstance> OnElementClicked
        {
            add
            {
                foreach (var element in _elementMap.Values)
                {
                    OnElementClickedBuffer += value;
                    element.OnClick += value;
                }
            }
            remove
            {
                foreach (var element in _elementMap.Values)
                {
                    OnElementClickedBuffer -= value;
                    element.OnClick -= value;
                }
            }
        }

        private event Action<WeaponInstance> OnElementMouseEnterBuffer;
        public event Action<WeaponInstance> OnElementMouseEnter
        {
            add
            {
                foreach (var element in _elementMap.Values)
                {
                    OnElementMouseEnterBuffer += value;
                    element.OnMouseEnter += value;
                }
            }
            remove
            {
                foreach (var element in _elementMap.Values)
                {
                    OnElementMouseEnterBuffer -= value;
                    element.OnMouseEnter -= value;
                }
            }
        }
        #endregion
    }
}