using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared;
namespace Client
{
    public class Looting : MonoBehaviour
    {
        public Item containedItem;
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                InventoryManager.AddItem(containedItem, MainManager.playerInventory);
                GameObject.Destroy(gameObject);
            }
        }
    }
}