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
        private Stack<ForgeItemInventoryUIElement> _pool = new Stack<ForgeItemInventoryUIElement>();
        private List<ForgeItemInventoryUIElement> _actives = new List<ForgeItemInventoryUIElement>();

        [SerializeField]
        private Selectable _defaultSelectable;

        [SerializeField]
        private ForgeItemInventoryUIElement _elementPrefab;
        [SerializeField]
        private Transform _container;

        #region Object Pooling
        private ForgeItemInventoryUIElement GetOrCreate()
        {
            ForgeItemInventoryUIElement element;
            if (_pool.Count > 0)
            {
                element = _pool.Pop();
                element.gameObject.SetActive(true);
            }
            else
            {
                element = Instantiate(_elementPrefab, _container);
            }
            element.gameObject.SetActive(true);
            element.transform.SetAsLastSibling();
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

        private GameObject _previouseSelection;

        private void OnEnable()
        {
            if (PlayerController.Instance) Open(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");

            var select = _actives.Count > 0 ? _actives.First().gameObject : _defaultSelectable.gameObject;
            _previouseSelection = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(select);
        }

        private void OnDisable()
        {
            if (PlayerController.Instance) Close(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");
            EventSystem.current?.SetSelectedGameObject(_previouseSelection);
        }

        private void InitializeNavigation()
        {
            int collumn = 4;

            for (int i = 0; i < _actives.Count; i++)
            {
                var button = _actives[i].Button;
                var nav = button.navigation;
                nav.mode = Navigation.Mode.Explicit;

                // 上のナビゲーション
                if (i / collumn == 0) // 一番上の行であれば自分を指定する。
                {
                    nav.selectOnUp = button;
                }
                else
                {
                    nav.selectOnUp = _actives[i - collumn].Button;
                }
                // 下のナビゲーション
                if (i / collumn == _actives.Count / collumn || i + collumn >= _actives.Count) // 一番下の行であれば自分を指定する。
                {
                    nav.selectOnDown = button;
                }
                else
                {
                    nav.selectOnDown = _actives[i + collumn].Button;
                }
                // 左のナビゲーション
                if (i % collumn == 0) // 一番左の列であれば自分を指定する。
                {
                    nav.selectOnLeft = button;
                }
                else
                {
                    nav.selectOnLeft = _actives[i - 1].Button;
                }
                // 右のナビゲーション
                if (i % collumn == collumn - 1 || i + 1 >= _actives.Count) // 一番右の列であれば自分を指定する。
                {
                    nav.selectOnRight = button;
                }
                else
                {
                    nav.selectOnRight = _actives[i + 1].Button;
                }
                button.navigation = nav;
            }
        }
    }
}