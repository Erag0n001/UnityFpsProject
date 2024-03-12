using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Weapon")]
public class WeaponStats : Item
{
    [Header("WeaponStats")]
    public float damage;
    public float minRange;
    public float maxRange;

    public Ammo[] allowedAmmoType;
    public Ammo currentAmmoType;
    public int currentMagSize;
    public int maxMagSize;

    public enum SpecialEffect { None, StaminaDrain, HealthDrain };
    public SpecialEffect specialEffect;
}