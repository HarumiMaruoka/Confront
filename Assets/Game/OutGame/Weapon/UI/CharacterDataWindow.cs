using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Weapon
{
    public class CharacterDataWindow : MonoBehaviour
    {
        [SerializeField]
        private InventoryWindow _inventoryWindow;

        // [Header("UI")]
        // ここにプレイヤーのステータス情報を表示するためのUIを追加する

        private void Start()
        {
            _inventoryWindow.OnElementMouseEnter += OnElementMouseEnter;
        }

        private void OnDestroy()
        {
            if (_inventoryWindow) _inventoryWindow.OnElementMouseEnter -= OnElementMouseEnter;
        }

        private void OnEnable()
        {
            Clear();
        }

        private void OnElementMouseEnter(WeaponInstance instance)
        {
            var player = PlayerController.Instance;
            if (!player)
            {
                Debug.LogError("PlayerController is not found.");
                return;
            }

            // 現在のプレイヤーとの差分ステータスを表示する処理を追加する。
        }

        private void Clear()
        {
            // ステータス表示をクリアする処理を追加する。
        }
    }
}