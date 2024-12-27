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
        private DropItemController _weaponDropItemPrefab;
        [SerializeField]
        private DropItemController _armorDropItemPrefab;
        [SerializeField]
        private DropItemController _actionItemDropItemPrefab;
        [SerializeField]
        private DropItemController _forgeItemDropItemPrefab;
        [SerializeField]
        private DropItemController _moneyDropItemPrefab;
        [SerializeField]
        private DropItemController _cardDropItemPrefab;

        private Dictionary<ItemType, HashSet<DropItemController>> _inactives = new Dictionary<ItemType, HashSet<DropItemController>>();

        private void Start()
        {
            _inactives.Add(ItemType.Weapon, new HashSet<DropItemController>());
            _inactives.Add(ItemType.Armor, new HashSet<DropItemController>());
            _inactives.Add(ItemType.ActionItem, new HashSet<DropItemController>());
            _inactives.Add(ItemType.ForgeItem, new HashSet<DropItemController>());
            _inactives.Add(ItemType.Money, new HashSet<DropItemController>());
            _inactives.Add(ItemType.Card, new HashSet<DropItemController>());

            CreateInitialObject();
        }

        public DropItemController Spawn(Vector3 position, ItemType type, int id, int amount)
        {
            DropItemController dropItem = GetDropItemFromPool(type);

            dropItem.transform.position = position;
            dropItem.gameObject.SetActive(true);
            dropItem.SetItem(position, type, id, amount);

            dropItem.OnComplete -= Despawn; // 重複登録を避けるため。
            dropItem.OnComplete += Despawn;

            return dropItem;
        }

        private DropItemController GetDropItemFromPool(ItemType type)
        {
            if (_inactives[type].Count > 0)
            {
                var dropItem = _inactives[type].First();
                _inactives[type].Remove(dropItem);
                return dropItem;
            }
            return Instantiate(GetPrefab(type), transform);
        }

        private DropItemController GetPrefab(ItemType type)
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

        private void Despawn(DropItemController dropItem)
        {
            dropItem.gameObject.SetActive(false);
            _inactives[dropItem.ItemType].Add(dropItem);
        }

        private void CreateInitialObject()
        {
            CreateInitialObject(ItemType.Weapon, 10);
            CreateInitialObject(ItemType.Armor, 10);
            CreateInitialObject(ItemType.ActionItem, 10);
            CreateInitialObject(ItemType.ForgeItem, 10);
            CreateInitialObject(ItemType.Money, 10);
            CreateInitialObject(ItemType.Card, 10);
        }

        private void CreateInitialObject(ItemType type, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var dropItem = Instantiate(GetPrefab(type), transform);
                dropItem.gameObject.SetActive(false);
                _inactives[type].Add(dropItem);
            }
        }
    }
}