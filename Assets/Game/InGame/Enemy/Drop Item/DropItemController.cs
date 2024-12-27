using Confront.Player;
using Confront.Utility;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Confront.DropItem
{
    public class DropItemController : MonoBehaviour
    {
        public ItemType ItemType;
        public int ItemID;
        public int Amount = 1;

        private PlayerController _player;

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

        [Header("その他")]
        [SerializeField]
        private AnimationCurve _alphaCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private float _elapsedTime = 0f;
        private float[] _defaultAlpha = null;
        private ParticleSystem[] _particleSystems;
        public event Action<DropItemController> OnComplete;

        private void Start()
        {
            _particleSystems = GetComponentsInChildren<ParticleSystem>();
            _defaultAlpha = new float[_particleSystems.Length];
            for (var i = 0; i < _particleSystems.Length; i++)
            {
                var main = _particleSystems[i].main;
                _defaultAlpha[i] = main.startColor.color.a;
            }
        }

        public void SetItem(Vector3 position, ItemType itemType, int itemID, int amount = 1)
        {
            ItemType = itemType;
            ItemID = itemID;
            transform.position = position;
            Amount = amount;
            _elapsedTime = 0f;
            _isPickedUp = false;

            if (_particleSystems != null && _defaultAlpha != null && _defaultAlpha.Length == _particleSystems.Length)
            {
                for (var i = 0; i < _particleSystems.Length; i++)
                {
                    var main = _particleSystems[i].main;
                    var color = main.startColor.color;
                    color.a = _defaultAlpha[i];
                    main.startColor = color;
                }
            }

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
                for (var i = 0; i < _particleSystems.Length; i++)
                {
                    var main = _particleSystems[i].main;
                    var color = main.startColor.color;
                    color.a = _defaultAlpha[i];
                    main.startColor = color;
                }

                _trajectoryCancellationTokenSource.Cancel();
                _isPickedUp = true;
                HandlePickup();
                return;
            }

            _elapsedTime += Time.deltaTime;

            var lifeTime = _alphaCurve.keys[_alphaCurve.length - 1].time;
            var alpha = _alphaCurve.Evaluate(_elapsedTime);

            foreach (var ps in _particleSystems)
            {
                var main = ps.main;
                var color = main.startColor.color;
                color.a = alpha;
                main.startColor = color;
            }

            if (_elapsedTime > lifeTime)
            {
                _trajectoryCancellationTokenSource.Cancel();
                _pickupCancellationTokenSource.Cancel();
                OnComplete?.Invoke(this);
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
            switch (ItemType)
            {
                case ItemType.Weapon: _player.WeaponInventory.AddWeapon(ItemID, Amount); break;
                case ItemType.Armor: break;
                case ItemType.Card: break;
                case ItemType.Money: break;
                case ItemType.ActionItem: _player.ActionItemInventory.AddItem(ItemID, Amount); break;
                case ItemType.ForgeItem: _player.ForgeItemInventory.Pickup(ItemID, Amount); break;
            }
        }

        public async UniTask PerformPickup(PlayerController player, float duration, float rotationDirection, float rotateSpeed = 1f, CancellationToken token = default)
        {
            // XZ平面上でプレイヤーを中心に回転しながらプレイヤーに向かって移動する
            var elapsedTime = 0f;

            rotationDirection = Mathf.Sign(rotationDirection);

            var height = transform.position.y;
            var playerXZ = new Vector3(player.transform.position.x, 0, player.transform.position.z) + _pickupOffset;
            var itemXZ = new Vector3(transform.position.x, 0, transform.position.z);
            var angleOffset = Mathf.Atan2(itemXZ.z - playerXZ.z, itemXZ.x - playerXZ.x);

            var initialRadius = Vector3.Distance(playerXZ, itemXZ);

            while (elapsedTime < duration && !token.IsCancellationRequested && player && this)
            {
                elapsedTime += Time.deltaTime;

                var radius = Mathf.Lerp(initialRadius, 0, elapsedTime / duration);
                var angle = angleOffset + elapsedTime * rotateSpeed * rotationDirection;

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
            _trajectoryCancellationTokenSource?.Cancel();
            _pickupCancellationTokenSource?.Cancel();
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