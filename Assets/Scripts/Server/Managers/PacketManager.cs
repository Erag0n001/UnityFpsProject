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
            Type toUse = typeof(PacketManager);
            MethodInfo methodInfo = toUse.GetMethod(packet.header);
            methodInfo.Invoke(packet.header, new object[] { packet, client });
        }
        public static void AddItemToInventory(Packet packet, Socializing client)
        {
            InventoryAddItem content = Serializer.ConvertBytesToObject<InventoryAddItem>(packet.contents);
            Inventory inventory = InventoryManager.FindInventory(Converter.SerializableInventoryToInventory(content.inventory));
            Printer.Log(inventory.content.Count.ToString());
            Printer.Log(content.item.amount.ToString());
            Printer.Log(inventory.id.ToString());
            InventoryManager.AddItem(content.item, inventory);
            Packet newPacket = new Packet("RequestInventoryContent",new InventoryPacket() { inventory = new SerializableInventory(inventory) }, true);
            RequestInventoryContent(newPacket,client);
        }
        public static void RequestInventoryContent(Packet packet, Socializing client)
        {
            InventoryPacket content = Serializer.ConvertBytesToObject<InventoryPacket>(packet.contents);
            Inventory inventory = InventoryManager.FindInventory(Converter.SerializableInventoryToInventory(content.inventory));
            client.AddToQueue(new Packet("RequestInventoryContent", new InventoryPacket() { inventory = new SerializableInventory(inventory) }, true));
        }
        public static void KeepAlivePacket(Packet packet, Socializing client){}
    }
}