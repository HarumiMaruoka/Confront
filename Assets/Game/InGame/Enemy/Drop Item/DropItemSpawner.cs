using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Confront.DropItem
{
    public class DropItemSpawner : MonoBehaviour
    {
        public static DropItemSpawner Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("DropItemSpawner is already in the scene.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

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

        private Dictionary<ItemType, HashSet<DropItem>> _inactives = new Dictionary<ItemType, HashSet<DropItem>>();

        private void Start()
        {
            _inactives.Add(ItemType.Weapon, new HashSet<DropItem>());
            _inactives.Add(ItemType.Armor, new HashSet<DropItem>());
            _inactives.Add(ItemType.ActionItem, new HashSet<DropItem>());
            _inactives.Add(ItemType.ForgeItem, new HashSet<DropItem>());
            _inactives.Add(ItemType.Money, new HashSet<DropItem>());
            _inactives.Add(ItemType.Card, new HashSet<DropItem>());
        }

        public DropItem Spawn(Vector3 position, ItemType type, int id)
        {
            DropItem dropItem = GetDropItemFromPool(type);

            dropItem.transform.position = position;
            dropItem.gameObject.SetActive(true);
            dropItem.SetItem(position, type, id);

            dropItem.OnComplete -= Despawn; // 重複登録を避けるため。
            dropItem.OnComplete += Despawn;

            return dropItem;
        }

        private DropItem GetDropItemFromPool(ItemType type)
        {
            if (_inactives[type].Count > 0)
            {
                var dropItem = _inactives[type].First();
                _inactives[type].Remove(dropItem);
                return dropItem;
            }
            return Instantiate(GetPrefab(type), transform);
        }

        private DropItem GetPrefab(ItemType type)
        {
            return type switch
            {
                ItemType.Weapon => _weaponDropItemPrefab,
                ItemType.Armor => _armorDropItemPrefab,
                ItemType.ActionItem => _actionItemDropItemPrefab,
                ItemType.ForgeItem => _forgeItemDropItemPrefab,
                ItemType.Money => _moneyDropItemPrefab,
                ItemType.Card => _cardDropItemPrefab,
                _ => null,
            };
        }

        public void Despawn(DropItem dropItem)
        {
            dropItem.gameObject.SetActive(false);
            _inactives[dropItem.ItemType].Add(dropItem);
        }
    }
}