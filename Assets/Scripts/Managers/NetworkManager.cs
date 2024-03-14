using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

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