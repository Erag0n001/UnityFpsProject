using System;
using System.Security.Cryptography;
using UnityEngine;
//Basic Item
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemObject : ScriptableObject ,IUse
{
    [Header("ItemStats")]
    public string iD;
    public string itemName;
    public string description;

    public double amount;
    public int maxAmount;

    public int rarety;
    public float weight;

    public Sprite icon;
    public enum ItemType { Item, Weapon, Ammo};
    public ItemType itemType;

    public bool activeEquipement;
    public Type activeEffect;

    public virtual void Use(){}
}

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "ScriptableObjects/RangedWeapon")]
public class RangedWeaponObject : ItemObject
{
    [Header("RangedWeapon")]
    public float damage;
    public float minRange;
    public float maxRange;

    public AmmoObject[] allowedAmmoType;
    public AmmoObject currentAmmoType;
    public int currentMagSize;
    public int maxMagSize;

    public enum SpecialEffect { None, StaminaDrain, HealthDrain };
    public SpecialEffect specialEffect;

    public GameObject weaponObject;
}

[CreateAssetMenu(fileName = "RangedAmmo", menuName = "ScriptableObjects/RangedAmmo")]
public class AmmoObject : ItemObject
{
    [Header("AmmoStats")]
    public float damage;

    public enum SpecialEffect {None,StaminaDrain, HealthDrain};
    public SpecialEffect specialEffect;
    public float specialEffectPotency;
}
