using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Confront.DropItem
{
    public class DropItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemType _itemType;

        [SerializeField]
        private DropItem _weaponDropItemPrefab;
        [SerializeField]
        private DropItem _armorDropItemPrefab;
        [SerializeField]
        private DropItem _actionItemDropItemPrefab;
        [SerializeField]
        private DropItem _forgeItemDropItemPrefab;
        [SerializeField]
        private DropItem _moneyDropItemPrefab;
        [SerializeField]
        private DropItem _cardDropItemPrefab;

        private Dictionary<DropItem, HashSet<DropItem>> _inactives = new Dictionary<DropItem, HashSet<DropItem>>();

        private void Start()
        {
            _inactives.Add(_weaponDropItemPrefab, new HashSet<DropItem>());
            _inactives.Add(_armorDropItemPrefab, new HashSet<DropItem>());
            _inactives.Add(_actionItemDropItemPrefab, new HashSet<DropItem>());
            _inactives.Add(_forgeItemDropItemPrefab, new HashSet<DropItem>());
            _inactives.Add(_moneyDropItemPrefab, new HashSet<DropItem>());
            _inactives.Add(_cardDropItemPrefab, new HashSet<DropItem>());
        }

        public DropItem Spawn(Vector3 position, ItemType type, int id)
        {
            DropItem dropItem = null;
            switch (type)
            {
                case ItemType.Weapon:
                    dropItem = GetDropItemFromPool(_weaponDropItemPrefab);
                    break;
                case ItemType.Armor:
                    dropItem = GetDropItemFromPool(_armorDropItemPrefab);
                    break;
                case ItemType.ActionItem:
                    dropItem = GetDropItemFromPool(_actionItemDropItemPrefab);
                    break;
                case ItemType.ForgeItem:
                    dropItem = GetDropItemFromPool(_forgeItemDropItemPrefab);
                    break;
                case ItemType.Money:
                    dropItem = GetDropItemFromPool(_moneyDropItemPrefab);
                    break;
                case ItemType.Card:
                    dropItem = GetDropItemFromPool(_cardDropItemPrefab);
                    break;
            }
            dropItem.transform.position = position;
            dropItem.gameObject.SetActive(true);
            dropItem.SetItem(position, type, id);

            dropItem.OnComplete -= Despawn; // 重複登録を避けるため。
            dropItem.OnComplete += Despawn;

            return dropItem;
        }

        private DropItem GetDropItemFromPool(DropItem weaponDropItemPrefab)
        {
            if (_inactives[weaponDropItemPrefab].Count > 0)
            {
                var dropItem = _inactives[weaponDropItemPrefab].First();
                _inactives[weaponDropItemPrefab].Remove(dropItem);
                return dropItem;
            }
            return Instantiate(weaponDropItemPrefab, transform);
        }

        public void Despawn(DropItem dropItem)
        {
            dropItem.gameObject.SetActive(false);
            _inactives[dropItem].Add(dropItem);
        }
    }
}