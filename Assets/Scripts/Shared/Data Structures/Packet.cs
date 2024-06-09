using System;
namespace Shared 
{
    [Serializable]
    public class Packet 
    {
        public string header;
        public byte[] contents;
        public bool requiresMainThread; 

        public static Packet CreateNewPacket(string header, object content, bool mainThread = false) 
        {
            Packet packet = new Packet();
            packet.header = header;
            packet.contents = Serializer.ConvertObjectToBytes(content);
            packet.requiresMainThread = mainThread;
            return packet;
        }
    }
}