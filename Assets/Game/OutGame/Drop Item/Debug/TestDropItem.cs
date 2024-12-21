using Confront.DropItem;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Debugger
{

    public class TestDropItem : MonoBehaviour
    {
        public int ID;
        public int Amount;
        public ItemType Type;
        public float PickupDistance = 1f;

        private void Update()
        {
            var sqrDistance = (transform.position - PlayerController.Instance.transform.position).sqrMagnitude;
            if (sqrDistance < PickupDistance * PickupDistance)
            {
                switch (Type)
                {
                    case ItemType.Weapon: PlayerController.Instance.WeaponInventory.AddWeapon(ID); break;
                    case ItemType.ActionItem: PlayerController.Instance.ActionItemInventory.AddItem(ID, Amount); break;
                    case ItemType.ForgeItem: PlayerController.Instance.ForgeItemInventory.Pickup(ID, Amount); break;
                }
                Destroy(gameObject);
            }
        }
    }
}