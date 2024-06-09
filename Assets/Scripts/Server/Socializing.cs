using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections;
using Shared;
using UnityEngine;
namespace Server
{
    public class Socializing
        {
        public Queue queue = new Queue();

        public TcpClient connection;
        public NetworkStream networkStream;

        public bool shouldClose;

        public StreamWriter streamWriter;
        public StreamReader streamReader;
        public Socializing(TcpClient connection)
        {
            this.connection = connection;
            networkStream = connection.GetStream();
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            Task.Run(delegate { Listen(); });
            Task.Run(delegate { Send(); });
            Task.Run(delegate { CheckConnection(); });
        }
        public void AddToQueue(Packet packet)
        {
            queue.Enqueue(packet);
        }
        public void Listen()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    string data = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(data)) { continue; }
                    else
                    {
                        Console.WriteLine(data);
                        Packet packet = Serializer.SerializeFromString<Packet>(data);
                        PacketManager.HandlePacket(packet, this);
                        shouldClose = false;
                    }
                }
            } catch(Exception ex) { Printer.LogError(ex.ToString()); }
            KillConnection();
        }
        public void Send()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    if (queue.Count > 0)
                    {
                        string data = Serializer.SerializeToString(queue.Dequeue());
                        streamWriter.WriteLine(data);
                        streamWriter.Flush();
                    }
                }
            } catch (Exception ex) { Printer.LogError(ex.ToString()); }
            KillConnection();
        }

        public void CheckConnection()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(15000);
                    if (shouldClose)
                    {
                        break;
                    }
                    else
                    {
                        shouldClose = true;
                    }
                }
            } catch(Exception ex) { Printer.LogError(ex.ToString()); }
            KillConnection();
        }

        public void KillConnection()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}