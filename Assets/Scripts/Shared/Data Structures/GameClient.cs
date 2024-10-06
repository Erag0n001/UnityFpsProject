using NUnit.Framework;
using Server;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Shared 
{
    public class GameClient 
    {
        public Server.Socializing socializing;
        public int playerID;
        public List<int> deadCreatureList = new List<int>();
        public List<int> deadPlayersList = new List<int>();
    }
}