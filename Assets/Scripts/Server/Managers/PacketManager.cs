using Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Server 
{
    public static class PacketManager
    {
        public static void HandlePacket(Packet packet, Socializing client)
        {
            Printer.LogWarning(packet.header);
            Type toUse = typeof(PacketManager);
            MethodInfo methodInfo = toUse.GetMethod(packet.header);
            methodInfo.Invoke(packet.header, new object[] { packet, client });
        }
        public static void AddItemToInventory(Packet packet, Socializing client)
        {
            List<object> content = Serializer.ConvertBytesToObject<List<object>>(packet.contents);
            Inventory inventory = InventoryManager.FindInventory((Inventory)content[0]);
            InventoryManager.AddItem((Item)content[1], inventory);
            Packet newPacket = Packet.CreateNewPacket("RequestInventoryContent", inventory, true);
            RequestInventoryContent(newPacket,client);
        }
        public static void RequestInventoryContent(Packet packet, Socializing client)
        {
            Inventory inventory = Serializer.ConvertBytesToObject<Inventory>(packet.contents);
            inventory = InventoryManager.FindInventory(inventory);
            client.AddToQueue(Packet.CreateNewPacket("RequestInventoryContent", inventory, true));
        }
        public static void KeepAlivePacket(Packet packet, Socializing client){}
    }
}