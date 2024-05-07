public static class NetworkManager
{
    public static string iPAddress = "192.168.2.213";
    public static int port = int.Parse("25555");

    public static Socializing listener;
    public static void Main()
    {
        listener = new Socializing(new(iPAddress, port));
    }
}