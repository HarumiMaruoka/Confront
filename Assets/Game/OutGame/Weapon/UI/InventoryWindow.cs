using Confront.Player;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        private List<InventoryWindowElement> _actives = new List<InventoryWindowElement>();
        private Dictionary<WeaponInstance, InventoryWindowElement> _elementMap = new Dictionary<WeaponInstance, InventoryWindowElement>();

        private PlayerController _player;
        private GameObject _previouseSelection;

        private void Start()
        {
            _player = PlayerController.Instance;
            if (_player == null)
            {
                Debug.LogError("PlayerController is not found.");
                return;
            }

            _inventory = _player.WeaponInventory;
            OnElementClicked += _player.EquipWeapon;
            Open(_inventory);
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                OnElementClicked -= _player.EquipWeapon;
            }

            if (_inventory != null)
            {
                _inventory.OnWeaponAdded -= OnWeaponAdded;
                _inventory.OnWeaponRemoved -= OnWeaponRemoved;
                _inventory.OnWeaponSwapped -= OnWeaponSwapped;
            }
        }

        private void OnEnable()
        {
            _previouseSelection = EventSystem.current.currentSelectedGameObject;
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
            EventSystem.current.SetSelectedGameObject(_previouseSelection);
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
                GetOrCreateElement(weapon);
            }

            InitializeNavigation();
        }


        private void Clear()
        {
            foreach (var element in _elementMap.Values)
            {
                element.gameObject.SetActive(false);
                _inactives.Enqueue(element);
            }
            _actives.Clear();
            _elementMap.Clear();
        }

        private InventoryWindowElement GetOrCreateElement(WeaponInstance weapon)
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
            _actives.Add(element);
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
        #endregion

        #region Inventory Events
        private void InitializeNavigation()
        {
            int collumn = 5;

            for (int i = 0; i < _actives.Count; i++)
            {
                var element = _actives[i].Button;
                var nav = element.navigation;
                nav.mode = Navigation.Mode.Explicit;

                // 上
                if (i / collumn == 0)
                {
                    nav.selectOnUp = element;
                }
                else
                {
                    nav.selectOnUp = _actives[i - collumn].Button;
                }
                // 下
                if (i / collumn == _actives.Count / collumn || i + collumn >= _actives.Count)
                {
                    nav.selectOnDown = element;
                }
                else
                {
                    nav.selectOnDown = _actives[i + collumn].Button;
                }
                // 左
                if (i % collumn == 0)
                {
                    nav.selectOnLeft = element;
                }
                else
                {
                    nav.selectOnLeft = _actives[i - 1].Button;
                }
                // 右
                if (i % collumn == collumn - 1 || i + 1 >= _actives.Count)
                {
                    nav.selectOnRight = element;
                }
                else
                {
                    nav.selectOnRight = _actives[i + 1].Button;
                }

                element.navigation = nav;
            }

            if (_actives.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(_actives[0].Button.gameObject);
            }
        }

        private void OnWeaponAdded(WeaponInstance weapon)
        {
            GetOrCreateElement(weapon);

            InitializeNavigation();
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
                OnElementClickedBuffer += value;
                foreach (var element in _elementMap.Values)
                {
                    element.OnClick += value;
                }
            }
            remove
            {
                OnElementClickedBuffer -= value;
                foreach (var element in _elementMap.Values)
                {
                    element.OnClick -= value;
                }
            }
        }

        private event Action<WeaponInstance> OnElementMouseEnterBuffer;
        public event Action<WeaponInstance> OnElementMouseEnter
        {
            add
            {
                OnElementMouseEnterBuffer += value;
                foreach (var element in _elementMap.Values)
                {
                    element.OnMouseEnter += value;
                }
            }
            remove
            {
                OnElementMouseEnterBuffer -= value;
                foreach (var element in _elementMap.Values)
                {
                    element.OnMouseEnter -= value;
                }
            }
        }
        #endregion
    }
}