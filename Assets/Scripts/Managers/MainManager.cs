using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainManager
{
    [Header("Managers")]
    public static RespawnManager respawnManager;
    public static LootManager lootManager;
    public static HotkeyManager hotkeyManager;
    public static ItemManager itemManager;
    public static InventoryManager inventoryManager;

    [Header("Items")]
    public static List<Item> itemList;
    public static GameObject[] lootList;

    [Header("Entities")]
    public static GameObject[] entityCreatureList;

    [Header("Player")]
    public static GameObject latestPlayerDeadBody;
    public static GameObject playerPrefab;
    public static GameObject alivePlayer;
    public static PlayerStatManager playerStatManager;
    public static CharacterMovement playerMovement;
    public static bool isPlayerAlive;
    public static GameObject playerInventory;
    public static HotbarManager playerHotbar;
    public static Quaternion PlayerRot;
    public static bool isPlayerInInventory;
}
