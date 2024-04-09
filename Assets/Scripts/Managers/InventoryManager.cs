using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public List<Item> itemStatsList= new List<Item>();
    public int currentItemId = 0;
    private void Awake()
    {
        MainManager.inventoryManager = this;
        MainManager.playerInventory = Resources.Load<GameObject>("UI/InventoryMain");
    }
    private void Start()
    {
        itemStatsList = new List<Item>();
    }
        
    public async Task<Item> CheckForExistingItem(int itemID)
    {
        return await Task.Run(() => {
            foreach (Item itemStats in itemStatsList)
            {
                if (itemStats.iD == itemID && itemStats.maxAmount != itemStats.amount)
                {
                    return itemStats;
                }
            }
            return null;
        });
    }

    public int AmountOfItemInInventory(int itemID)
    {
        int amount = 0;
        foreach (Item itemStats in itemStatsList)
        {
            if(itemStats.iD == itemID)
            {
                amount += itemStats.amount;
            }
        }
        return amount;
    }

    public void UpdateInventory()
    {

    }

    //Remove a single item
    public async Task<bool> RemoveItem(Item item)
    {
        int amountOfItem = AmountOfItemInInventory(item.iD);
        if(amountOfItem >= item.amount)
        {
            //Declares a new task and grabs it's value
            Task<Item> itemTask = this.CheckForExistingItem(item.iD);
            Item inventoryItem = await itemTask;
            if (inventoryItem.amount - item.amount < 0)
            {
                int amountToLoop = (int)Mathf.Ceil(item.amount / item.maxAmount);
                for(int i = 0;amountToLoop >= i; i++)
                {
                    int itemLeft = (inventoryItem.amount - item.amount) * -1;
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
        return false;
    }

    //Remove multiple item
    public void RemoveItem(Item[] itemArray)
    {
        foreach (Item item in itemArray) 
        {
#pragma warning disable CS4014
            RemoveItem(item);
#pragma warning restore CS4014
        }
    }
    //Remove a slot
    public int RemoveItemEntry(Item item, int itemLeft)
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
    public async void AddItem(Item item)
    {
        //Declares a new task and grabs it's value
        Task<Item> itemTask = this.CheckForExistingItem(item.iD);
        Item inventoryItem = await itemTask;
        if (inventoryItem == null)
        {
            item.amount = CreateNewItemEntry(item);
            inventoryItem = itemStatsList.Last();
        }
        if (inventoryItem.amount + item.amount >= inventoryItem.maxAmount)
        {
            item.amount -= inventoryItem.maxAmount - inventoryItem.amount;
            inventoryItem.amount = inventoryItem.maxAmount;
            int amountToLoop = (int)Mathf.Ceil(item.amount / item.maxAmount);
            for (int i = 0; amountToLoop >= i; i++)
            {
                item.amount = CreateNewItemEntry(item);
            }
        }
        else
        {
            inventoryItem.amount += item.amount;
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
    public int CreateNewItemEntry(Item item)
    {
        if(item.amount !< item.maxAmount)
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