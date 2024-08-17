using NUnit.Framework.Internal;
using Shared;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
namespace Client
{
    public static class NetworkManager
    {
        public static string iPAddress = "192.168.0.151";
        public static int port = int.Parse("25555");

        public static Socializing listener;
        public static void StartConnections()
        {
            if (!Client.MainManager.IsServer)
            {
                Printer.Log("Connection initiated");
                new Socializing(new(iPAddress, port));
            }
        }
    }
}