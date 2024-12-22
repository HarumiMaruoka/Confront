using Confront.Player;
using Confront.Utility;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Confront.DropItem
{
    public class DropItem : MonoBehaviour
    {
        [NonSerialized]
        public ItemType ItemType;

        private PlayerController _player;
        private int _itemID;

        private bool _isPickedUp = false;

        [Header("斜方投射")]
        [SerializeField]
        private float _minInitialSpeed = 10f;
        [SerializeField]
        private float _maxInitialSpeed = 20f;
        [SerializeField]
        private float _minAngle = 45f;
        [SerializeField]
        private float _maxAngle = 60f;
        [SerializeField]
        private float _gravity = 9.8f;
        [SerializeField]
        private LayerMask _layerMask;
        private CancellationTokenSource _trajectoryCancellationTokenSource;

        [Header("アイテム取得")]
        [SerializeField]
        private float _pickupDistance = 1.5f;

        [Header("アイテム取得演出")]
        [SerializeField]
        private float _pickupDuration = 1f;
        [SerializeField]
        private float _pickupRotationDirection = 1f;
        [SerializeField]
        private float _pickupRotateSpeed = 1f;
        [SerializeField]
        private Vector3 _pickupOffset = new Vector3(0, 1, 0);
        private CancellationTokenSource _pickupCancellationTokenSource;

        public event Action<DropItem> OnComplete;

        public void SetItem(Vector3 position, ItemType itemType, int itemID)
        {
            ItemType = itemType;
            _itemID = itemID;
            transform.position = position;
            _isPickedUp = false;

            _trajectoryCancellationTokenSource = new CancellationTokenSource();
            _pickupCancellationTokenSource = new CancellationTokenSource();

            _player = PlayerController.Instance;
            if (!_player) Debug.LogError("PlayerController is not found.");

            var randomInitialSpeed = UnityEngine.Random.Range(_minInitialSpeed, _maxInitialSpeed);
            var randomAngle = UnityEngine.Random.Range(_minAngle, _maxAngle);

            var token = _trajectoryCancellationTokenSource.Token;
            this.SimulateTrajectoryAsync(randomInitialSpeed, randomAngle, _gravity, _layerMask, token).Forget();
        }

        private void Update()
        {
            if (!_player) return;
            if (_isPickedUp) return;

            var sqrDistance = Vector3.SqrMagnitude(_player.transform.position - transform.position);
            if (sqrDistance < _pickupDistance * _pickupDistance)
            {
                _trajectoryCancellationTokenSource.Cancel();
                _isPickedUp = true;
                HandlePickup();
            }
        }

        private async void HandlePickup()
        {
            var token = _pickupCancellationTokenSource.Token;
            await PerformPickup(_player, _pickupDuration, _pickupRotationDirection, _pickupRotateSpeed, token);
            ProcessItemPickup();
            OnComplete?.Invoke(this);
        }

        private void ProcessItemPickup()
        {
            Debug.Log($"ここにアイテムを取得する処理を書く。");
        }

        public async UniTask PerformPickup(PlayerController player, float duration, float rotationDirection, float rotateSpeed = 1f, CancellationToken token = default)
        {
            // XZ平面上でプレイヤーを中心に回転しながらプレイヤーに向かって移動する
            var elapsedTime = 0f;
            rotationDirection = Mathf.Sign(rotationDirection);

            var height = transform.position.y;
            var playerXZ = new Vector3(player.transform.position.x, 0, player.transform.position.z) + _pickupOffset;
            var itemXZ = new Vector3(transform.position.x, 0, transform.position.z);
            var initialRadius = Vector3.Distance(playerXZ, itemXZ);

            while (elapsedTime < duration && !token.IsCancellationRequested && player && this)
            {
                elapsedTime += Time.deltaTime;

                var radius = Mathf.Lerp(initialRadius, 0, elapsedTime / duration);
                var angle = elapsedTime * rotateSpeed * rotationDirection;

                var x = Mathf.Cos(angle) * radius;
                var y = Mathf.Lerp(height, player.transform.position.y, elapsedTime / duration);
                var z = Mathf.Sin(angle) * radius;

                var playerPositionXZ = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                transform.position = playerPositionXZ + new Vector3(x, y, z) + _pickupOffset;
                await UniTask.Yield(token);
            }
        }

        private void OnDestroy()
        {
            _trajectoryCancellationTokenSource.Cancel();
            _pickupCancellationTokenSource.Cancel();
        }

        public void SkipTrajectory()
        {
            _trajectoryCancellationTokenSource.Cancel();
            _isPickedUp = true;
            OnComplete?.Invoke(this);
        }

        public void SkipPickup()
        {
            _pickupCancellationTokenSource.Cancel();
            ProcessItemPickup();
            OnComplete?.Invoke(this);
        }
    }
}