using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public int currentSlot;

    public Item[] hotbar = new Item[10];

    private void Awake()
    {
        MainManager.playerHotbar = this;
    }

    public void UseItem()
    {
        if(hotbar[currentSlot] != null)
        {
            hotbar[currentSlot].Use();
        }
    }
}
