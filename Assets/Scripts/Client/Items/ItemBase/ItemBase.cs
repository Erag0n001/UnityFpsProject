using System;
using System.Security.Cryptography;
using UnityEngine;
public class Item : IUse
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

public class RangedWeapon : Item
{
    [Header("RangedWeaponStats")]
    public float damage;
    public float minRange;
    public float maxRange;

    public Ammo[] allowedAmmoType;
    public Ammo currentAmmoType;
    public int currentMagSize;
    public int maxMagSize;

    public enum SpecialEffect { None, StaminaDrain, HealthDrain };
    public SpecialEffect specialEffect;

    public GameObject weaponObject;
    public override void Use()
    {
        
    }
}

public class Ammo : Item
{
    [Header("AmmoStats")]
    public float damage;

    public enum SpecialEffect {None,StaminaDrain, HealthDrain};
    public SpecialEffect specialEffect;
    public float specialEffectPotency;
}
public interface IUse
{
    void Use();
}