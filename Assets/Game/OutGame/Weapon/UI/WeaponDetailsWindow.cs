using System;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponDetailsWindow : MonoBehaviour
    {
        [SerializeField]
        private InventoryWindow _inventoryWindow;

        [Header("Details UI")]
        [SerializeField]
        private TMPro.TextMeshProUGUI _name;
        // ここに武器の詳細情報を表示するためのUIを追加する

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
            _name.text = instance.Data.Name;
            // ここに武器の詳細情報を表示する処理を追加する
        }

        private void Clear()
        {
            _name.text = string.Empty;
            // ここに武器の詳細情報をクリアする処理を追加する
        }
    }
}