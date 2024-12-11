using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Item
{
    public class TestItem : MonoBehaviour
    {
        [SerializeField]
        private int _id;
        [SerializeField]
        private int _amount;

        [SerializeField]
        private float _radius = 1.5f;

        private void Start()
        {
            if (!ItemManager.ItemSheet.Contains(_id))
            {
                Debug.LogError($"Item ID {_id} is not found in the ItemSheet.");
                return;
            }
        }

        private void Update()
        {
            if ((PlayerController.Instance.transform.position - this.transform.position).sqrMagnitude < _radius * _radius)
            {
                var item = ItemManager.ItemSheet.GetItem(_id);
                var remainingAmount = PlayerController.Instance.ItemInventory.AddItem(item, _amount);
                if (remainingAmount > 0)
                {
                    _amount = remainingAmount;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}