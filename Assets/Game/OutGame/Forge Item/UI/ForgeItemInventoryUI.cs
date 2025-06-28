using Confront.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.ForgeItem
{
    public class ForgeItemInventoryUI : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;
        [SerializeField]
        private ForgeItemInventoryUIElement _elementPrefab;
        [SerializeField]
        private Selectable _defaultSelectable;
        [SerializeField]
        private ForgeItemDescriptionView _descriptionView;

        private Stack<ForgeItemInventoryUIElement> _pool = new Stack<ForgeItemInventoryUIElement>();
        private List<ForgeItemInventoryUIElement> _actives = new List<ForgeItemInventoryUIElement>();

        #region Object Pooling
        private ForgeItemInventoryUIElement GetOrCreate()
        {
            ForgeItemInventoryUIElement element;
            if (_pool.Count > 0)
            {
                element = _pool.Pop();
                element.gameObject.SetActive(true);
                element.transform.SetAsLastSibling();
            }
            else
            {
                element = Instantiate(_elementPrefab, _container);
                element.OnSelected += OnSelected;
            }

            _actives.Add(element);
            return element;
        }

        private void ReturnToPool(ForgeItemInventoryUIElement element)
        {
            element.gameObject.SetActive(false);
            _pool.Push(element);
        }
        #endregion

        public void Open(ForgeItemInventory inventory)
        {
            foreach (var item in inventory)
            {
                var element = GetOrCreate();
                element.ForgeItemData = item.Key;
                element.Count = item.Value;
                inventory.OnCountChanged += element.UpdateView;
            }

            InitializeNavigation();

            if (_actives.Count > 0)
            {
                _actives.First().Button.Select();
                OnSelected(_actives.First());
            }
            else
            {
                _defaultSelectable.Select();
                _descriptionView.Clear();
            }
        }

        public void Close(ForgeItemInventory inventory)
        {
            foreach (var element in _actives)
            {
                inventory.OnCountChanged -= element.UpdateView;
                ReturnToPool(element);
            }
            _actives.Clear();
        }

        private GameObject _previousSelection;

        private void OnEnable()
        {
            if (PlayerController.Instance) Open(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");

            var select = _actives.Count > 0 ? _actives.First().gameObject : _defaultSelectable.gameObject;
            _previousSelection = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(select);
        }

        private void OnDisable()
        {
            if (PlayerController.Instance) Close(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");
            EventSystem.current?.SetSelectedGameObject(_previousSelection);
        }

        // VerticalLayoutナビゲーションを手動で設定する。
        private void InitializeNavigation()
        {
            for (int i = 0; i < _actives.Count; i++)
            {
                var button = _actives[i].Button;
                var nav = button.navigation;
                nav.mode = Navigation.Mode.Explicit;

                // 上のナビゲーション
                if (i == 0) // 一番上の行であれば自分を指定する。
                {
                    nav.selectOnUp = button;
                }
                else // そうでなければ一つ前のボタンを指定する。
                {
                    nav.selectOnUp = _actives[i - 1].Button;
                }
                // 下のナビゲーション
                if (i == _actives.Count - 1) // 一番下の行であれば自分を指定する。
                {
                    nav.selectOnDown = button;
                }
                else // そうでなければ一つ後のボタンを指定する。
                {
                    nav.selectOnDown = _actives[i + 1].Button;
                }
                // 左のナビゲーションは常に自分を指定する。
                nav.selectOnLeft = button;
                // 右のナビゲーションは常に自分を指定する。
                nav.selectOnRight = button;

                button.navigation = nav;
            }
        }

        private void OnSelected(ForgeItemInventoryUIElement element)
        {
            _descriptionView.UpdateView(element?.ForgeItemData);
        }
    }
}