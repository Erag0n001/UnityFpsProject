using Shared;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading.Tasks;

namespace Server
{
    public static class MainManager
    {
        static public void Startup() 
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Printer.Log("Server application starting");
            NetworkManager.StartConnections();
            Task.Run(ConsoleManager.ConsoleListenForInput);
            Task.Run(CreatureManager.SyncCreaturesWithClient);
            Application.targetFrameRate = 60;
            //Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
            Client.MainManager.IsServer = true;
            SceneManager.LoadScene(2);
        }
        public static List<Item> itemList = new List<Item>();
        public static List<Inventory> inventories = new List<Inventory>();
        public static int inventoryCount = 0;
        public static List<GameClient> clientList = new List<GameClient>();
        public static bool isPaused = false;
    }
}