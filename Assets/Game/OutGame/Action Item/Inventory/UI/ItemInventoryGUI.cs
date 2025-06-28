using Confront.GUI;
using Confront.Input;
using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Confront.ActionItem
{
    public class ItemInventoryGUI : MonoBehaviour
    {
        private GameObject _previousSelection;

        [SerializeField]
        private SlotUI _hotBarTop;
        [SerializeField]
        private SlotUI _hotBarBottom;
        [SerializeField]
        private SlotUI _hotBarLeft;
        [SerializeField]
        private SlotUI _hotBarRight;

        [SerializeField]
        private DescriptionView _descriptionView; // アイテムの説明を表示するUI

        [SerializeField]
        private SlotUI _prefab;
        [SerializeField]
        private Transform _slotContainer;

        // ホバー中のスロット（アイテムを移動する際に使用する。）
        public SlotUI HoveringSlotUI;

        public Vector2 HoveringOffset = new Vector2(10, 10);

        private List<SlotUI> _slots = new List<SlotUI>();

        // フォーカス中のスロット（アイテムを使用する際に使用する。）
        public SlotUI FocusedSlotUI
        {
            get
            {
                if (InputDeviceManager.LastInputDevice == InputDevice.KeyboardMouse)
                {
                    return MouseOverlappingSlot();
                }
                else
                {
                    return EventSystem.current.currentSelectedGameObject?.GetComponent<SlotUI>();
                }
            }
        }

        public void Open(ActionItemInventory inventory)
        {
            // 足りない分は生成し、余分な分は非表示にする。
            for (int i = 0; i < inventory.Count; i++)
            {
                SlotUI slotUI;
                if (i < _slots.Count)
                {
                    slotUI = _slots[i];
                    _slots[i].gameObject.SetActive(true);
                }
                else
                {
                    slotUI = Instantiate(_prefab, _slotContainer);
                    slotUI.OnSlotSelected += OnSelectionChanged;

                    _slots.Add(slotUI);
                }
                slotUI.Slot = inventory[i];
            }
            // 超過分は非表示にする。
            for (int i = inventory.Count; i < _slots.Count; i++)
            {
                _slots[i].gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (PlayerController.Instance)
            {
                _previousSelection = EventSystem.current.currentSelectedGameObject;
                Open(PlayerController.Instance.ActionItemInventory);
                EventSystem.current.SetSelectedGameObject(_slots[0].gameObject);
            }
        }

        private void Start()
        {
            var player = PlayerController.Instance;
            if (player)
            {
                Open(player.ActionItemInventory);
                InitializeHotBar(player.HotBar);
                InitializeNavigation();
                InitializeDescriptionAction();
                EventSystem.current.SetSelectedGameObject(_slots[0].gameObject);
            }
            else
            {
                Debug.LogError("PlayerController is not exist.");
            }
            HoveringSlotUI.Slot = new InventorySlot();
        }

        private void Update()
        {
            HandleInput();
            UpdateHoveringSlotPosition();
        }

        private void OnDisable()
        {
            if (HoveringSlotUI.Slot.Item != null)
            {
                var remainingItemCount = PlayerController.Instance.ActionItemInventory.AddItem(HoveringSlotUI.Slot.Item, HoveringSlotUI.Slot.Count);
                if (remainingItemCount != 0) Debug.LogWarning("インベントリがいっぱいです。");
            }
            HoveringSlotUI.Slot.Item = null;
            HoveringSlotUI.Slot.Count = 0;
            // TODO:アイテムが消滅しないようにする。

            if (_previousSelection)
            {
                EventSystem.current?.SetSelectedGameObject(_previousSelection);
            }
            else
            {
                Debug.LogWarning("Previous selection is null");
            }
        }

        private void UpdateHoveringSlotPosition()
        {
            if (HoveringSlotUI?.Slot?.Item != null)
            {
                if (InputDeviceManager.LastInputDevice == InputDevice.KeyboardMouse)
                {
                    HoveringSlotUI.transform.position = UnityEngine.Input.mousePosition + (Vector3)HoveringOffset;
                }
                else
                {
                    if (FocusedSlotUI)
                    {
                        HoveringSlotUI.transform.position = FocusedSlotUI.transform.position + (Vector3)HoveringOffset;
                    }
                    else
                    {
                        Debug.Log("Focused is null");
                    }
                }
            }
        }

        private void HandleInput()
        {
            var hoveringID = HoveringSlotUI?.Slot?.Item?.ID;
            var focusedID = FocusedSlotUI?.Slot?.Item?.ID;
            if (PlayerInputHandler.OutGameInput.Click.triggered)
            {
                if (hoveringID == focusedID)
                {
                    TransferAnyItem(); // 同じアイテムの場合は、Focusedに転送できるだけ転送する。
                }
                else
                {
                    Swap(); // 異なるアイテムの場合は、アイテムを交換する。
                }
            }
            if (PlayerInputHandler.OutGameInput.RightClick.triggered)
            {
                if (hoveringID == focusedID || FocusedSlotUI?.Slot?.Item == null)
                {
                    TransferOneItem();
                }
                else if (FocusedSlotUI?.Slot?.Item != null && HoveringSlotUI?.Slot?.Item == null)
                {
                    // フォーカス中のスロットが空で、ホバー中のスロットが空でない場合、フォーカス中のスロットからホバー中のスロットに半分移動する。
                    TransferHalfItem();
                }
            }
        }

        private void Swap()
        {
            if (FocusedSlotUI == null) return;

            var tempItem = HoveringSlotUI?.Slot?.Item;
            var tempCount = HoveringSlotUI?.Slot?.Count;

            HoveringSlotUI.Slot.Item = FocusedSlotUI?.Slot?.Item;
            HoveringSlotUI.Slot.Count = (FocusedSlotUI?.Slot?.Count).GetValueOrDefault();
            HoveringSlotUI?.UpdateUI(HoveringSlotUI?.Slot);

            FocusedSlotUI.Slot.Item = tempItem;
            FocusedSlotUI.Slot.Count = tempCount.GetValueOrDefault();
            FocusedSlotUI?.UpdateUI(FocusedSlotUI?.Slot);
        }

        private void TransferOneItem()
        {
            if (HoveringSlotUI.Slot == null || HoveringSlotUI.Slot.Item == null) return;
            if (FocusedSlotUI == null || FocusedSlotUI.Slot == null) return;

            if (FocusedSlotUI.Slot.Item == null) // フォーカス中のスロットが空の場合
            {
                FocusedSlotUI.Slot.Item = HoveringSlotUI.Slot.Item;
                FocusedSlotUI.Slot.Count = 1;
                HoveringSlotUI.Slot.Count--;
                return;
            }

            // IDが同じかどうかチェック
            if (HoveringSlotUI.Slot.Item.ID != FocusedSlotUI.Slot.Item.ID) return;
            // スタックが最大かどうかチェック
            if (FocusedSlotUI.Slot.Count >= FocusedSlotUI.Slot.Item.MaxStack) return;
            // スタックが0かどうかチェック
            if (HoveringSlotUI.Slot.Count <= 0) return;

            FocusedSlotUI.Slot.Count++;
            HoveringSlotUI.Slot.Count--;
        }

        private void TransferHalfItem()
        {
            var transferAmount = FocusedSlotUI.Slot.Count / 2;
            HoveringSlotUI.Slot.Item = FocusedSlotUI.Slot.Item;
            HoveringSlotUI.Slot.Count = transferAmount;
            FocusedSlotUI.Slot.Count -= transferAmount;
        }

        private void TransferAnyItem()
        {
            if (HoveringSlotUI.Slot == null || HoveringSlotUI.Slot.Item == null) return;
            if (FocusedSlotUI == null || FocusedSlotUI.Slot == null || FocusedSlotUI.Slot.Item == null) return;

            // IDが同じかどうかチェック
            if (HoveringSlotUI.Slot.Item.ID != FocusedSlotUI.Slot.Item.ID) return;

            var remainingCapacity = FocusedSlotUI.Slot.Item.MaxStack - FocusedSlotUI.Slot.Count;
            var transferAmount = Mathf.Min(remainingCapacity, HoveringSlotUI.Slot.Count);

            FocusedSlotUI.Slot.Count += transferAmount;
            HoveringSlotUI.Slot.Count -= transferAmount;
        }

        private SlotUI MouseOverlappingSlot()
        {
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = UnityEngine.Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);
            foreach (var result in results)
            {
                var slotUI = result.gameObject.GetComponent<SlotUI>();
                if (slotUI) return slotUI;
            }
            return null;
        }

        private void InitializeHotBar(HotBar hotBar)
        {
            _hotBarTop.Slot = hotBar.GetSlot(Direction.Top);
            _hotBarBottom.Slot = hotBar.GetSlot(Direction.Bottom);
            _hotBarLeft.Slot = hotBar.GetSlot(Direction.Left);
            _hotBarRight.Slot = hotBar.GetSlot(Direction.Right);
        }

        private void InitializeNavigation()
        {
            if (_slots == null || _slots.Count == 0) return;

            var columns = 4;
            var rows = 5;

            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var navigation = slot.Button.navigation;
                navigation.mode = Navigation.Mode.Explicit;

                // 4で割り切れる場合は、leftOnSelectに自分を設定する。
                if (i % columns == 0)
                {
                    navigation.selectOnLeft = slot.Button;
                }
                else
                {
                    navigation.selectOnLeft = _slots[i - 1].Button;
                }

                // 4の剰余が1の場合は、rightOnSelectに自分を設定する。
                if (i % columns == columns - 1)
                {
                    navigation.selectOnRight = slot.Button;
                }
                else
                {
                    navigation.selectOnRight = _slots[i + 1].Button;
                }

                // 4で割った商が0の場合は、upOnSelectに自分を設定する。
                if (i / columns == 0)
                {
                    navigation.selectOnUp = slot.Button;
                }
                else
                {
                    navigation.selectOnUp = _slots[i - 4].Button;
                }

                // 4で割った商が4の場合は、downOnSelectにHotBarの上を設定する。
                if (i / columns == rows - 1)
                {
                    navigation.selectOnDown = _hotBarTop.Button;
                }
                else
                {
                    navigation.selectOnDown = _slots[i + 4].Button;
                }

                slot.Button.navigation = navigation;
            }

            // HotBarのナビゲーション設定
            // ========== HotBarTop ========== //
            var hotBarTopNavigation = _hotBarTop.Button.navigation;
            hotBarTopNavigation.mode = Navigation.Mode.Explicit;

            hotBarTopNavigation.selectOnUp = _slots[columns * (rows - 1) + 1].Button; // Slotsの左下の１つ右を設定する。
            hotBarTopNavigation.selectOnDown = _hotBarBottom.Button;
            hotBarTopNavigation.selectOnLeft = _hotBarLeft.Button;
            hotBarTopNavigation.selectOnRight = _hotBarRight.Button;
            _hotBarTop.Button.navigation = hotBarTopNavigation;
            // ========== HotBarBottom ========== //
            var hotBarBottomNavigation = _hotBarBottom.Button.navigation;
            hotBarBottomNavigation.mode = Navigation.Mode.Explicit;

            hotBarBottomNavigation.selectOnUp = _hotBarTop.Button;
            hotBarBottomNavigation.selectOnDown = _hotBarBottom.Button;
            hotBarBottomNavigation.selectOnLeft = _hotBarLeft.Button;
            hotBarBottomNavigation.selectOnRight = _hotBarRight.Button;
            _hotBarBottom.Button.navigation = hotBarBottomNavigation;
            // ========== HotBarLeft ========== //
            var hotBarLeftNavigation = _hotBarLeft.Button.navigation;
            hotBarLeftNavigation.mode = Navigation.Mode.Explicit;

            hotBarLeftNavigation.selectOnUp = _hotBarTop.Button;
            hotBarLeftNavigation.selectOnDown = _hotBarBottom.Button;
            hotBarLeftNavigation.selectOnLeft = _hotBarLeft.Button;
            hotBarLeftNavigation.selectOnRight = _hotBarRight.Button;
            _hotBarLeft.Button.navigation = hotBarLeftNavigation;
            // ========== HotBarRight ========== //
            var hotBarRightNavigation = _hotBarRight.Button.navigation;
            hotBarRightNavigation.mode = Navigation.Mode.Explicit;

            hotBarRightNavigation.selectOnUp = _hotBarTop.Button;
            hotBarRightNavigation.selectOnDown = _hotBarBottom.Button;
            hotBarRightNavigation.selectOnLeft = _hotBarLeft.Button;
            hotBarRightNavigation.selectOnRight = _hotBarRight.Button;
            _hotBarRight.Button.navigation = hotBarRightNavigation;
            // ============================================================= //
        }

        private void InitializeDescriptionAction()
        {
            _hotBarTop.OnSlotSelected += OnSelectionChanged;
            _hotBarBottom.OnSlotSelected += OnSelectionChanged;
            _hotBarLeft.OnSlotSelected += OnSelectionChanged;
            _hotBarRight.OnSlotSelected += OnSelectionChanged;

            _descriptionView.UpdateView(null);
        }

        private void OnSelectionChanged(SlotUI slot)
        {
            if (slot == null || slot.Slot == null || slot.Slot.Item == null) return;
            _descriptionView.UpdateView(slot.Slot.Item);
        }
    }
}