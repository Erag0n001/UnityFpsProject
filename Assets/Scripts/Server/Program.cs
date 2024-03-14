
using System.Net;
using System.Net.Sockets;

namespace Test
{
    public class Test 
    {
        public static IPAddress iPAddress = IPAddress.Parse("0.0.0.0");
        public static int port = int.Parse("25555");

        public static TcpListener server;
        public static void Main()
        {
            server = new TcpListener(iPAddress, port);
            server.Start();
            
            while(true) {ListenForConnections(); }
        }
        public static void ListenForConnections()
        {
            TcpClient newTCP = server.AcceptTcpClient();
            Console.WriteLine(newTCP.ToString());
            Socializing socializing = new Socializing(newTCP);
        }



}
}