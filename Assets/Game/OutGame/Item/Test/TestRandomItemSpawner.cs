using System;
using UnityEngine;

namespace Confront.Item.Test
{
    public class TestRandomItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _randomItems;

        private void OnMouseDown()
        {
            var randomItem = _randomItems[UnityEngine.Random.Range(0, _randomItems.Length)];
            Instantiate(randomItem, transform.position, Quaternion.identity);
        }
    }
}