using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class LootManager : MonoBehaviour
{

    public void Awake()
    {
        MainManager.lootManager= this;
        MainManager.lootList = Resources.LoadAll<GameObject>("Prefab/Loots");
    }
    public void SpawnLoot(Vector3 spawnLocation, int lootQuality)
    {
        Item item = MainManager.itemManager.FetchItem();
        item.amount = 70;
        GameObject loot = MainManager.lootList[0];
        loot = GameObject.Instantiate(loot);
        loot.GetComponent<Looting>().containedItem = item;
        loot.transform.position = spawnLocation;
    }
}
