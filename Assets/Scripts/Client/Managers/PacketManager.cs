using Shared;
using System;
using System.Reflection;
namespace Client 
{
    public static class PacketManager
    {
        public static void HandlePacket(Packet packet, Socializing client)
        {
            Action toDo = () =>
            {
                Printer.LogWarning(packet.header);
                Type toUse = typeof(PacketManager);
                MethodInfo methodInfo = toUse.GetMethod(packet.header);
                methodInfo.Invoke(packet.header, new object[] { packet , client });
            };
            if (packet.requiresMainThread == true)
            {
                MainManager.unityMainThreadDispatcher.Enqueue(toDo);
            } else
            {
                toDo.Invoke();
            }
        }
        public static void RequestInventoryContent(Packet packet, Socializing client)
        {
            Inventory inventory = Serializer.ConvertBytesToObject<Inventory>(packet.contents);
            InventoryManager.UpdateInventory(inventory);
        }
        public static void KeepAlivePacket(Packet packet, Socializing client) { }
    }
}