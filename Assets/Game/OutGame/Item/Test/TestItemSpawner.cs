using System;
using UnityEngine;

namespace Confront.Item.Test
{
    public class TestItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _itemPrefab;

        private void Start()
        {
            Spawn();
        }

        private void OnMouseDown()
        {
            Spawn();
        }

        private void Spawn()
        {
            var item = Instantiate(_itemPrefab, transform.position, Quaternion.identity);
        }
    }
}