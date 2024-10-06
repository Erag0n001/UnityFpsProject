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
                InventoryManager.AddItem(containedItem, MainManager.mainPlayer.playerInventory);
                GameObject.Destroy(gameObject);
            }
        }
    }
}