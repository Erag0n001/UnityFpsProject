using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gun
{
    public static LayerMask layerMask = 7;
    public static void Use(RangedWeapon weapon, Transform transform)
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, weapon.minRange, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
            Debug.Log("Hit");
        }
    }
}
