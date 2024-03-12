using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Ammo")]
public class Ammo : Item
{
    [Header("AmmoStats")]
    public float damage;

    public enum SpecialEffect {None,StaminaDrain, HealthDrain};
    public SpecialEffect specialEffect;
    public float specialEffectPotency;
}