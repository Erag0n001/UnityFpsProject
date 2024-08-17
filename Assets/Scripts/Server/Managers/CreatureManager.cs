using NUnit.Framework;
using Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Server
{
    static public class CreatureManager
    {
        static public SyncAllCreaturesPacket syncAllCreaturesPacket;
        static public void SyncCreaturesWithClient()
        {
            try
            {
                while (true)
                {
                    foreach (GameClient gameClient in (MainManager.clientList))
                    {
                        syncAllCreaturesPacket = Client.CreatureManager.SyncCreaturesWithClient(gameClient);
                        gameClient.socializing.AddToQueue(new Packet("SyncCreaturesFromServer", syncAllCreaturesPacket));
                    }
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex) { Printer.LogError(ex.ToString()); }
        }
    }
}