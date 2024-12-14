using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.ForgeItem
{
    public class ForgeItemInventoryUI : MonoBehaviour
    {
        private Stack<ForgeItemInventoryUIElement> _pool = new Stack<ForgeItemInventoryUIElement>();
        private HashSet<ForgeItemInventoryUIElement> _actives = new HashSet<ForgeItemInventoryUIElement>();

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

        private void OnEnable()
        {
            if (PlayerController.Instance) Open(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");
        }

        private void OnDisable()
        {
            if (PlayerController.Instance) Close(PlayerController.Instance.ForgeItemInventory);
            else Debug.LogError("PlayerController is not found.");
        }
    }
}