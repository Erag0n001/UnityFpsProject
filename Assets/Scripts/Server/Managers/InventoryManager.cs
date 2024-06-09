namespace Server 
{
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    public static class InventoryManager
    {
        public static Inventory CreateNewIDForInventory(Inventory inventory) 
        {
            MainManager.inventoryCount++;
            inventory.id = MainManager.inventoryCount;
            MainManager.inventories.Add(inventory);
            return inventory;
        }
        public static Inventory FindInventory(Inventory inventoryClient) 
        {
            Printer.Log("Inventory came in with +" + inventoryClient.id);
            Inventory inventory = null;
            if (inventoryClient.id == 0)
            {
                CreateNewIDForInventory(inventoryClient);
                return inventoryClient;
            }
            foreach (Inventory inv in MainManager.inventories) 
            {
                if(inv.id == inventoryClient.id) 
                {
                    inventory = inv;
                    return inv;
                }
            }
            return CreateNewIDForInventory(new Inventory());
        }

        static Item CheckForExistingItem(string itemID, Inventory inventory)
        {
            foreach (Item itemStats in inventory.content)
            {
                if (itemStats.iD == itemID && itemStats.maxAmount != itemStats.amount)
                {
                    return itemStats;
                }
            }
            return null;
        }

        public static double AmountOfItemInInventory(string itemID, Inventory inventory)
        {
            double amount = 0;
            foreach (Item itemStats in inventory.content)
            {
                if (itemStats.iD == itemID)
                {
                    amount += itemStats.amount;
                }
            }
            return amount;
        }

        //Remove a single item
        public static bool RemoveItem(Item item, Inventory inventory)
        {
            double amountOfItem = AmountOfItemInInventory(item.iD, inventory);
            if (amountOfItem >= item.amount)
            {
                //Declares a new task and grabs it's value
                Item inventoryItem = CheckForExistingItem(item.iD, inventory);
                if (inventoryItem.amount - item.amount < 0)
                {
                    int amountToLoop = (int)Math.Ceiling(item.amount / item.maxAmount);
                    for (int i = 0; amountToLoop >= i; i++)
                    {
                        double itemLeft = (inventoryItem.amount - item.amount) * -1;
                        inventoryItem.amount = inventoryItem.maxAmount;
                        RemoveItemEntry(item, itemLeft, inventory);
                    }
                }
                else
                {
                    inventoryItem.amount -= item.amount;
                }
            }
            else
            {
                Printer.Log("Not enough item in inventory");
                return false;
            }
            return false;
        }

        //Remove multiple item
        public static void RemoveItem(Item[] itemArray, Inventory inventory)
        {
            foreach (Item item in itemArray)
            {
                RemoveItem(item, inventory);
            }
        }
        //Remove a slot
        public static double RemoveItemEntry(Item item, double itemLeft, Inventory inventory)
        {
            if (itemLeft! > item.maxAmount)
            {
                itemLeft -= inventory.content.Find(test => test.iD == item.iD).amount;
                inventory.content.Remove(item);
                return 0;
            }
            else
            {
                inventory.content.Add(item);
                inventory.content.Last().amount = item.maxAmount;
                return itemLeft - 50;
            }
        }

        //Add single item
        public static void AddItem(Item item, Inventory inventory)
        {
            Item inventoryItem = CheckForExistingItem(item.iD, inventory);
            if (inventoryItem == null)
            {
                item.amount = CreateNewItemEntry(item, inventory);
                inventoryItem = inventory.content.Last();
            }
            if (inventoryItem.amount + item.amount >= inventoryItem.maxAmount)
            {
                item.amount -= inventoryItem.maxAmount - inventoryItem.amount;
                inventoryItem.amount = inventoryItem.maxAmount;
                double amountToLoop = Math.Ceiling(item.amount / item.maxAmount);
                for (int i = 0; amountToLoop > i; i++)
                {
                    item.amount = CreateNewItemEntry(item, inventory);
                }
            }
            else
            {
                inventoryItem.amount += item.amount;
            }
        }
        //Add multiple item
        public static void AddItem(Item[] itemArray, Inventory inventory)
        {
            foreach (Item item in itemArray)
            {
                AddItem(item, inventory);
            }
        }
        //Create new inventory slot
        public static double CreateNewItemEntry(Item item, Inventory inventory)
        {
            if (item.amount <= item.maxAmount)
            {
                inventory.content.Add(ItemManager.CreateNewItem(item));
                return 0;
            }
            else
            {
                inventory.content.Add(ItemManager.CreateNewItem(item));
                inventory.content.Last().amount = item.maxAmount;
                item.amount -= item.maxAmount;
                return item.amount;
            }
        }
    }
}