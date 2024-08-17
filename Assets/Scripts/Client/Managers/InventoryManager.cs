using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shared;
namespace Client
{
    public class InventoryLocation 
    {
        public GameObject inventoryObject;
        public Transform grid;
    }
    public static class InventoryManager
    {
        public static Queue<Inventory> inventoryRequestQueue = new Queue<Inventory>();
        public enum InventoryPossibleLocations { Left, Right, Central }
        public static Inventory[] inventoryInUse = new Inventory[3] 
        {null,null,null};
        public static InventoryLocation[] inventoryLocation = new InventoryLocation[3]
        {new InventoryLocation(), new InventoryLocation(), new InventoryLocation()};

        public static GameObject canvas;
        public static GameObject inventoryPrefab;
        public static GameObject inventorySlotPrefab;

        static InventoryManager() 
        {
            canvas = GameObject.FindWithTag("Canvas");
            inventoryPrefab = Resources.Load<GameObject>("Prefab/UI/Inventory/InventoryMain");
            inventorySlotPrefab = Resources.Load<GameObject>("Prefab/UI/Inventory/InventorySlot");
    }
        public static void InventoryUpdate(Inventory inventory, int objectIndex)
        {
            inventoryLocation[objectIndex].grid = inventoryLocation[objectIndex].inventoryObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
            inventory.content = inventory.content.OrderBy(item => item.iD).ToList();
            foreach (Item item in inventory.content)
            {
                GameObject slot = GameObject.Instantiate(inventorySlotPrefab);
                slot.transform.SetParent(inventoryLocation[objectIndex].grid);
                slot.transform.localScale = new Vector3(1, 1, 1);
                Sprite sprite;
                if (ItemManager.textureDic.TryGetValue(item.iconID, out sprite)) 
                {
                    slot.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                } else
                {
                    slot.GetComponent<UnityEngine.UI.Image>().sprite = ItemManager.textureDic["Dummy"];
                }
            }
        }

        public static void InventoryUIShow(Inventory inventory, InventoryPossibleLocations location, bool shouldSendPacket = true)
        {
            if (shouldSendPacket)
            {
                RequestInventoryFromServer(inventory);
            }
            int index = (int)location;
            if (inventoryInUse[index] == null)
            {
                inventoryLocation[index].inventoryObject = GameObject.Instantiate(inventoryPrefab);
                inventoryLocation[index].inventoryObject.transform.SetParent(canvas.transform, false);
                inventoryInUse[index] = inventory;
                MainManager.playerCameraControl.UnlockCursor();
                InventoryUpdate(inventory, index);
            }
            else
            {
                GameObject.Destroy(inventoryLocation[index].inventoryObject);
                MainManager.playerCameraControl.LockCursor();
                inventoryInUse[(int)location] = null;
            }
        }

        public static void InventoryUIShowPlayer(bool shouldSendPacket = true)
        {
            if (shouldSendPacket)
            {
                RequestInventoryFromServer(MainManager.playerInventory);
            }
            if (inventoryInUse[0] == null)
            {
                inventoryLocation[0].inventoryObject = GameObject.Instantiate(inventoryPrefab);
                inventoryLocation[0].inventoryObject.transform.SetParent(canvas.transform, false);
                inventoryInUse[0] = MainManager.playerInventory;
                MainManager.playerCameraControl.UnlockCursor();
                InventoryUpdate(MainManager.playerInventory, 0);
            }
            else
            {
                GameObject.Destroy(inventoryLocation[0].inventoryObject);
                MainManager.playerCameraControl.LockCursor();
                inventoryInUse[0] = null;
            }
        }

        public static void UpdateInventory(Inventory inventory) 
        {
            Inventory inventoryinQueue = inventoryRequestQueue.Dequeue();
            if(inventoryinQueue == MainManager.playerInventory) 
            {
                MainManager.playerInventory = inventory;
                InventoryUIShowPlayer(false);
                InventoryUIShowPlayer(false);
                return;
            }
            else 
            {
                foreach (Inventory inv in MainManager.inventoryList)
                {
                    if (inv == inventoryinQueue) 
                    {
                        inventoryinQueue = inventory;
                        return;
                    }
                }
            }
        }

        public static void RequestInventoryFromServer(Inventory inventoryToRequest) 
        {
            inventoryRequestQueue.Enqueue(inventoryToRequest);
            MainManager.Socializing.AddToQueue(new Packet("RequestInventoryContent", new InventoryPacket() { inventory = new SerializableInventory(inventoryToRequest) }));
        }
        public static void AddItem(Item item, Inventory inventory)
        {
            inventoryRequestQueue.Enqueue(inventory);
            MainManager.Socializing.AddToQueue(new Packet("AddItemToInventory",new InventoryAddItem(){ inventory = new SerializableInventory(inventory), item = item}));
        }

    }
}