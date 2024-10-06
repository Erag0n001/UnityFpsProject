using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
namespace Shared 
{
    [Serializable]
    public class Packet 
    {
        public string header;
        public byte[] contents;
        public bool requiresMainThread; 

        public Packet(string header, object content, bool mainThread = false) 
        {
            this.header = header;
            this.contents = Serializer.ConvertObjectToBytes(content);
            this.requiresMainThread = mainThread;
        }
    }

    public class KeepAlivePacket 
    {

    }
    [Serializable]
    public class InventoryPacket
    {
        public SerializableInventory inventory;
    }
    [Serializable]
    public class InventoryAddItem 
    {
        public SerializableInventory inventory;
        public Item item;
    }
    [Serializable]
    public class CreaturePacket
    {
        public Creature creature;
    }
    [Serializable]
    public class SyncAllCreaturesPacket 
    {
        public List<int> deadCreatures = new List<int>();
        public CreaturePacket[] creatures = new CreaturePacket[0];
    }
    [Serializable]
    public class PlayerPacket
    {
        public Player player;
        public SerializableVector3 position;
        public SerializableVector4 rotation;
    }
    public class SyncAllPlayersPacket 
    {
        public PlayerPacket[] players = new PlayerPacket[0];
        public List<int> deadPlayers = new List<int>();
    }
}