using System.IO;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class Socializing
{
    public TcpClient connection;
    public NetworkStream networkStream;

    public StreamWriter streamWriter;
    public StreamReader streamReader;

    public bool shouldClose;
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
                    UnityEngine.Debug.Log(data);
                    shouldClose = false;
                }
            }
        }
        catch (Exception ex) { Debug.Log(ex); }
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
        }
        catch (Exception ex) { }
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
        }
        catch (Exception ex) { Console.WriteLine(ex); }
        KillConnection();
    }

    public void KillConnection()
    {
        connection.Close();
        connection.Dispose();
    }
}