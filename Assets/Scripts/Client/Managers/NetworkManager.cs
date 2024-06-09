using System.Threading.Tasks;
using System;
using UnityEngine;
using Shared;
using System.Collections;
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
        //public static IEnumerable GetInventoryFromServer()
        //{
        //    MainManager.Socializing.queue.Enqueue(new Packet => );
        //}
    }
}