using System.Net;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using Shared;
namespace Server
{
    public static class NetworkManager 
    {
        public static IPAddress iPAddress = IPAddress.Parse("0.0.0.0");
        public static int port = int.Parse("25555");

        public static TcpListener server;
        public static void StartConnections()
        {
            server = new TcpListener(iPAddress, port);
            server.Start();
            Task.Run(() =>
            {
                while (true) { ListenForConnections(); }
            });
        }
        public static void ListenForConnections()
        {
            TcpClient newTCP = server.AcceptTcpClient();
            Console.WriteLine(newTCP.ToString());
            Socializing socializing = new Socializing(newTCP);
            MainManager.clientList.Add(new GameClient() {socializing = socializing});
        }
    }
}