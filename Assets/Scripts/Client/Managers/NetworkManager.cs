using Shared;
namespace Client
{
    public static class NetworkManager
    {
        public static string iPAddress = "192.168.0.151";
        public static int port = int.Parse("25555");

        public static Socializing listener;
        public static void Main()
        {
            Printer.Log("Connection initiated");
            new Socializing(new(iPAddress, port));
        }
    }
}