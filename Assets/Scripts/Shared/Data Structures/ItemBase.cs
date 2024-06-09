using System;
namespace Shared
{
    [Serializable]public class Item : IUse
    {
        public string iD;
        public string itemName;
        public string description;

        public double amount;
        public int maxAmount;

        public int rarety;
        public float weight;

        public string iconID;
        public enum ItemType { Item, Weapon, Ammo };
        public ItemType itemType;

        public bool activeEquipement;
        public Type activeEffect;

        public virtual void Use() { }
    }

    public class RangedWeapon : Item
    {
        public float damage;
        public float minRange;
        public float maxRange;

        public Ammo[] allowedAmmoType;
        public Ammo currentAmmoType;
        public int currentMagSize;
        public int maxMagSize;

        public enum SpecialEffect { None, StaminaDrain, HealthDrain };
        public SpecialEffect specialEffect;

        public string weaponObjectID;
        public override void Use()
        {

        }
    }

    public class Ammo : Item
    {
        public float damage;

        public enum SpecialEffect { None, StaminaDrain, HealthDrain };
        public SpecialEffect specialEffect;
        public float specialEffectPotency;
    }
    public interface IUse
    {
        void Use();
    }
    public class ItemType
    {
        public enum Type { Item, Weapon, Ammo };
        public Type itemType;
    }
}