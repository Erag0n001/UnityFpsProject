using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public int iD;
    public string itemName;
    public string description;

    public int amount;
    public int maxAmount;

    public int rarety;
    public float weight;
    public enum ItemType { Weapons, RangedWeapons, Tools, Item }
    public ItemType itemType;

    public Texture2D icon;
}
