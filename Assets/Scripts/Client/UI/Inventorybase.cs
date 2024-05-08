using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("InventoryMain")]
    public List<Item> itemStatsList= new List<Item>();
    public Item[] hotbar;
    public HotbarManager hotbarManager;
    public bool inventoryBool;

    public GameObject inventoryObject;
    [Header("InventoryUI")]
    public GameObject canvas;
    public GameObject inventoryPrefab;
    public GameObject inventorySlotPrefab;

    private void Start()
    {
        itemStatsList = new List<Item>();
        hotbarManager = MainManager.playerHotbar;
        hotbar = hotbarManager.hotbar;
        inventoryBool = true;
    }

    public void InventoryUpdate()
    {
        Transform grid = inventoryObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
        itemStatsList = itemStatsList.OrderBy(item => item.iD).ToList();
        foreach(Item item in itemStatsList)
        {
            GameObject slot = GameObject.Instantiate(inventorySlotPrefab);
            slot.transform.SetParent(grid);
            slot.transform.localScale = new Vector3(1,1,1);
            slot.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
        }
    }

    public virtual void InventoryUIShow()
    {
        if (inventoryBool)
        {
            inventoryObject = GameObject.Instantiate(inventoryPrefab);
            inventoryObject.transform.SetParent(this.gameObject.transform, false);
            inventoryBool = false;
            MainManager.playerCameraControl.UnlockCursor();
            InventoryUpdate();
        }
        else
        {
            GameObject.Destroy(inventoryObject);
            MainManager.playerCameraControl.LockCursor();
            inventoryBool = true;
        }
    }
        
    Item CheckForExistingItem(string itemID)
    {
        foreach (Item itemStats in itemStatsList)
        {
            if (itemStats.iD == itemID && itemStats.maxAmount != itemStats.amount)
            {
                return itemStats;
            }
        }
        return null;
    }

    public double AmountOfItemInInventory(string itemID)
    {
        double amount = 0;
        foreach (Item itemStats in itemStatsList)
        {
            if(itemStats.iD == itemID)
            {
                amount += itemStats.amount;
            }
        }
        return amount;
    }

    //Remove a single item
    public bool RemoveItem(Item item)
    {
        double amountOfItem = AmountOfItemInInventory(item.iD);
        if(amountOfItem >= item.amount)
        {
            //Declares a new task and grabs it's value
            Item inventoryItem = CheckForExistingItem(item.iD);
            if (inventoryItem.amount - item.amount < 0)
            {
                int amountToLoop = (int)Math.Ceiling(item.amount / item.maxAmount);
                for(int i = 0;amountToLoop >= i; i++)
                {
                    double itemLeft = (inventoryItem.amount - item.amount) * -1;
                    inventoryItem.amount = inventoryItem.maxAmount;
                    RemoveItemEntry(item, itemLeft);
                }
            }
            else
            {
                inventoryItem.amount -= item.amount;
            }
        }
        else
        {
            print("Not enough item in inventory");
            return false;
        }
        if(!inventoryBool)
        {
            InventoryUIShow();
            InventoryUIShow();
        }
        return false;
    }

    //Remove multiple item
    public void RemoveItem(Item[] itemArray)
    {
        foreach (Item item in itemArray) 
        {
            RemoveItem(item);
        }
    }
    //Remove a slot
    public double RemoveItemEntry(Item item, double itemLeft)
    {
        if (itemLeft !> item.maxAmount)
        {
            itemLeft -= itemStatsList.Find(test => test.iD == item.iD).amount;
            itemStatsList.Remove(item);
            return 0;
        }
        else
        {
            itemStatsList.Add(item);
            itemStatsList.Last().amount = item.maxAmount;
            return itemLeft - 50;
        }
    }

    //Add single item
    public void AddItem(Item item)
    {
        //Declares a new task and grabs it's value
        Item inventoryItem = CheckForExistingItem(item.iD);
        if (inventoryItem == null)
        {
            item.amount = CreateNewItemEntry(item);
            inventoryItem = itemStatsList.Last();
        }
        if (inventoryItem.amount + item.amount >= inventoryItem.maxAmount)
        {
            item.amount -= inventoryItem.maxAmount - inventoryItem.amount;
            inventoryItem.amount = inventoryItem.maxAmount;
            double amountToLoop = Math.Ceiling(item.amount / item.maxAmount);
            for (int i = 0; amountToLoop > i; i++)
            {
                item.amount = CreateNewItemEntry(item);
            }
        }
        else
        {
            inventoryItem.amount += item.amount;
        }

        if(item.itemType == Item.ItemType.Weapon)
        {
            for(int i = 0; i >= 10;i++)
            {
                if(hotbar[i] == null)
                {
                    hotbar[i] = item;
                }
            }
        }
        if(!inventoryBool)
        {
            InventoryUIShow();
            InventoryUIShow();
        }
    }
    //Add multiple item
    public void AddItem(Item[] itemArray)
    {
        foreach (Item item in itemArray)
        {
            AddItem(item);
        }
    }
    //Create new inventory slot
    public double CreateNewItemEntry(Item item)
    {
        if(item.amount <= item.maxAmount)
        {
            itemStatsList.Add(MainManager.itemManager.CreateNewItem(item));
            return 0;
        } else
        {
            itemStatsList.Add(MainManager.itemManager.CreateNewItem(item));
            itemStatsList.Last().amount = item.maxAmount;
            item.amount -= 50;  
            return item.amount;
        }   
    }
}