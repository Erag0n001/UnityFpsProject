using NUnit.Framework;
using Shared;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server
{
    public static class MainManager
    {
        public static void Startup() 
        {
        
        }
        public static List<Item> itemList = new List<Item>();
        public static List<Inventory> inventories = new List<Inventory>();
        public static int inventoryCount = 0;
        public static List<TcpClient> clientList = new List<TcpClient>();
    }
}