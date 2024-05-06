using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    private void Awake()
    {
        MainManager.itemManager = this;
        List<Item> objectList = LoadAllItems();
        MainManager.itemList = objectList;
        NetworkManager.Main();
    }
    public Item FetchItem()
    {
        return CreateNewItem(MainManager.itemList[0]);
    }

    public Item CreateNewItem(Item itemToAssign)
    {
        PropertyInfo[] properties = itemToAssign.GetType().GetProperties();
        Item newItem = new Item();
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
        Item newItem = new Item();
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

    public List<Item> LoadAllItems()
    {
        List<Item> objectList = new List<Item>();
        ScriptableObject[] scriptableObjects = Resources.LoadAll<ScriptableObject>("Items");
        print(scriptableObjects.Length);
        for(int i = 0;i < scriptableObjects.Length;i++)
        {
            string converter;
            switch(scriptableObjects[i].GetType().ToString())
            {
                case "ItemObject": 
                    converter = JsonUtility.ToJson(scriptableObjects[i]);
                    objectList.Add(JsonUtility.FromJson<Item>(converter));
                    break;
                case "RangedWeaponStatsObject":
                    converter = JsonUtility.ToJson(scriptableObjects[i]);
                    objectList.Add(JsonUtility.FromJson<RangedWeapon>(converter));
                    break;
                case "AmmoObject":
                    converter = JsonUtility.ToJson(scriptableObjects[i]);
                    objectList.Add(JsonUtility.FromJson<Ammo>(converter));
                    break;
                default:
                    Debug.LogError($"{scriptableObjects[i].name} was not able to find a match. This shouldn't happen\nThe following type was detected for the item: {scriptableObjects[i].GetType().ToString()}");
                    break;
            }
        }
        
        return objectList;
    }
}