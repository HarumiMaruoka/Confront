using Confront.DropItem;
using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Debugger
{
    public class TestDropItemSpawner2 : MonoBehaviour
    {
        [SerializeField]
        private DropItemData[] _datas;

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
            var position = (Vector2)transform.position + _spawnOffset;
            var data = _datas[UnityEngine.Random.Range(0, _datas.Length)];
            DropItemSpawner.Instance.Spawn(position, data.Type, data.ID, data.Amount);
        }
    }
}