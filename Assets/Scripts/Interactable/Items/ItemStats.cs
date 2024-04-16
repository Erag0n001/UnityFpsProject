using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    [Header("ItemStats")]
    public int iD;
    public string itemName;
    public string description;

    public double amount;
    public int maxAmount;

    public int rarety;
    public float weight;

    public Texture2D icon;
    public enum ItemType { Item, Weapon, Ammo};
    public ItemType itemType;
}
