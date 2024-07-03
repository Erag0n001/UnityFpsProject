using Shared;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    public static class MainManager
    {
        [Header("Managers")]
        public static RespawnManager respawnManager;
        public static LootManager lootManager;
        public static HotkeyManager hotkeyManager;

        [Header("Items")]
        public static List<Item> itemList;
        public static GameObject[] lootList;

        [Header("Entities")]
        public static GameObject[] entityCreatureList;
        public static Inventory[] inventoryList;

        [Header("Player")]
        public static GameObject latestPlayerDeadBody;
        public static GameObject playerPrefab;
        public static GameObject alivePlayer;
        public static PlayerStatManager playerStatManager;
        public static CharacterMovement playerMovement;
        public static bool isPlayerAlive;
        public static HotbarManager playerHotbar;
        public static Quaternion PlayerRot;
        public static bool isPlayerInInventory;
        public static Inventory playerInventory = new Inventory() { id = -1 };
        public static CameraControl playerCameraControl;

        [Header("Network")]
        public static Socializing Socializing;
        [Header("Misc")]
        public static UnityMainThreadDispatcher unityMainThreadDispatcher;
        public static bool IsServer;
    }
}