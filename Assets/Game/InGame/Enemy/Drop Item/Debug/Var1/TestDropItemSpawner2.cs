using Confront.DropItem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Confront.Debugger
{
    public class TestDropItemSpawner2 : MonoBehaviour
    {
        [SerializeField]
        private DropItemController[] _prefabs;

        [SerializeField]
        private Vector2 _spawnOffset = new Vector2(0, 0.7f);
        [SerializeField]
        private float _interval = 0.1f;

        private bool _isMouseDown = false;
        private float _timer = 0;

        private void OnMouseDown() => _isMouseDown = true;
        private void OnMouseUp() => _isMouseDown = false;

        private void Update()
        {
            if (!_isMouseDown) return;

            _timer += Time.deltaTime;
            if (_timer >= _interval)
            {
                _timer = 0;
                Spawn();
            }
        }

        private void Spawn()
        {
            var prefab = _prefabs[UnityEngine.Random.Range(0, _prefabs.Length)];
            var instance = Instantiate(prefab, transform.position + new Vector3(_spawnOffset.x, _spawnOffset.y, 0), Quaternion.identity);
            instance.OnComplete += OnComplete;
            instance.SetItem(transform.position, prefab.ItemType, prefab.ItemID);
        }

        private void OnComplete(DropItemController controller)
        {
            controller.OnComplete -= OnComplete;
            Destroy(controller.gameObject);
        }
    }
}