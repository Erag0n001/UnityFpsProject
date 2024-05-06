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
using Unity.Mathematics;

public class InventoryManager : MonoBehaviour
{
    [Header("InventoryMain")]
    public List<Item> itemStatsList= new List<Item>();
    public Item[] hotbar;
    public HotbarManager hotbarManager;
    private bool inventoryBool;

    private GameObject inventoryObject;
    [Header("InventoryUI")]
    public GameObject canvas;
    public GameObject inventoryPrefab;
    public GameObject inventorySlotPrefab;
    bool needUpdating;
    
    private void Awake()
    {
        MainManager.inventoryManager = this;
        MainManager.playerInventory = Resources.Load<GameObject>("UI/InventoryMain");
    }

    private void Start()
    {
        itemStatsList = new List<Item>();
        hotbarManager = MainManager.playerHotbar;
        hotbar = hotbarManager.hotbar;
        inventoryBool = true;
        AddItem(MainManager.itemManager.CreateNewItem(MainManager.itemList[1]));
    }

    public void InventoryUpdate()
    {
        Transform grid = GameObject.FindWithTag("InventoryContent").transform; print(grid.name);
        itemStatsList = itemStatsList.OrderBy(item => item.iD).ToList();
        foreach(Item item in itemStatsList)
        {
            GameObject slot = GameObject.Instantiate(inventorySlotPrefab);
            slot.transform.SetParent(grid);
            slot.transform.localScale = new Vector3(1,1,1);
            slot.GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
        }
    }

    public void InventoryUIShow()
    {
        if (inventoryBool)
        {
            inventoryObject = GameObject.Instantiate(inventoryPrefab);
            inventoryObject.transform.SetParent(canvas.transform, false);
            inventoryBool = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            InventoryUpdate();
        }
        else
        {
            GameObject.Destroy(inventoryObject);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
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
        InventoryUIShow();
        InventoryUIShow();
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
                print($"Creating new stack, amount left = {amountToLoop}, math = {item.amount / item.maxAmount}");
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
        InventoryUIShow();
        InventoryUIShow();
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