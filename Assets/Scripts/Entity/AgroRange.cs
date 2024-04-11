using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CreatureAI creatureAI = gameObject.GetComponentInParent<CreatureAI>();
        creatureAI.SetAgro(other);
    }
}
