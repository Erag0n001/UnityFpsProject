using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Server
{
public class Socializing
    {
        public TcpClient connection;
        public NetworkStream networkStream;

        public bool shouldClose;

        public StreamWriter streamWriter;
        public StreamReader streamReader;
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
                        shouldClose = false;
                    }
                }
            } catch(Exception ex) { Console.WriteLine(ex); }
            KillConnection();
        }
        public void Send()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                    string data = "test";
                    streamWriter.WriteLine(data);
                    streamWriter.Flush();
                }
            } catch (Exception ex) { Console.WriteLine(ex); }
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
            } catch(Exception ex) { Console.WriteLine(ex); }
            KillConnection();
        }

        public void KillConnection()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}