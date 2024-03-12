using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Item emptyItem;
    private void Awake()
    {
        Item[] objectList;
        MainManager.itemManager = this;
        objectList = Resources.LoadAll<Item>("Prefab/Items");
        MainManager.itemList = objectList;
        emptyItem = Resources.Load<Item>("Prefab/Items/Empty");
    }

    public Item FetchItem()
    {
        return CreateNewItem((MainManager.itemList[0]));
    }

    public Item CreateNewItem(Item itemToAssign)
    {
        Item newItem = (Item)ScriptableObject.CreateInstance(typeof(Item));
        newItem.iD = itemToAssign.iD;
        newItem.itemName = itemToAssign.itemName;
        newItem.description = itemToAssign.description;
        newItem.amount = itemToAssign.amount;
        newItem.maxAmount = itemToAssign.maxAmount;
        newItem.rarety = itemToAssign.rarety;
        newItem.weight = itemToAssign.weight;
        newItem.icon = itemToAssign.icon;
        return newItem;
    }

    public Item CreateNewEmptyItem(Item itemToAssign)
    {
        Item newItem = (Item)ScriptableObject.CreateInstance(typeof(Item));
        newItem.iD = itemToAssign.iD;
        newItem.itemName = itemToAssign.itemName;
        newItem.description = itemToAssign.description;
        newItem.amount = 0;
        newItem.maxAmount = itemToAssign.maxAmount;
        newItem.rarety = itemToAssign.rarety;
        newItem.weight = itemToAssign.weight;
        newItem.icon = itemToAssign.icon;
        return newItem;
    }

}