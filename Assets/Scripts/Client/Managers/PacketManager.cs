using Shared;
using System;
using System.Numerics;
using System.Reflection;
namespace Client 
{
    public static class PacketManager
    {
        public static void HandlePacket(Packet packet, Socializing client)
        {
            Action toDo = () =>
            {
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
            InventoryPacket inventory = Serializer.ConvertBytesToObject<InventoryPacket>(packet.contents);
            InventoryManager.UpdateInventory(Converter.SerializableInventoryToInventory(inventory.inventory));
        }
        public static void SyncCreaturesFromServer(Packet packet, Socializing clientt)
        {
            SyncAllCreaturesPacket syncAllCreaturesPacket = Serializer.ConvertBytesToObject<SyncAllCreaturesPacket>(packet.contents);
            CreatureManager.SyncAllCreaturesFromServer(syncAllCreaturesPacket);
        }
        public static void KeepAlivePacket(Packet packet, Socializing client) { }
    }
}