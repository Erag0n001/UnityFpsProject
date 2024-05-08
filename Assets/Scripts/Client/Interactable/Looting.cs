using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looting : MonoBehaviour
{
    public Item containedItem;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            MainManager.playerInventory.AddItem(containedItem);
            GameObject.Destroy(gameObject);
        }
    }
}
