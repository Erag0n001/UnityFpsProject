using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
namespace Client 
{
    static class CreatureManager
    {
        static public List<int> deadCreatures = new List<int>();
        static public List<int> deadPlayers = new List<int>();
        static SyncAllCreaturesPacket packet;

        public static SyncAllCreaturesPacket SyncCreaturesWithClient(GameClient client)
        {
            packet = new SyncAllCreaturesPacket();
            List<CreaturePacket> creaturelist = new List<CreaturePacket>();
            foreach (Creature creature in MainManager.creatureList.ToList())
            {
                //if (creature.creatureAI.creature.stats.needsUpdating)
                //{
                    CreaturePacket creaturePacket = new CreaturePacket();
                    creaturePacket.creature = creature;
                    creaturelist.Add(creaturePacket);
                    creature.creatureAI.creature.needsUpdating = false;
                    
                //}
            }
            foreach(int i in deadCreatures) 
            {
                if (!client.deadCreatureList.Contains(i)) 
                {
                    client.deadCreatureList.Add(i);
                    packet.deadCreatures.Add(i);
                }
            }
            packet.creatures = creaturelist.ToArray();
            return packet;
        }

        public static void SyncCreaturesFromServer(SyncAllCreaturesPacket syncAllCreaturesPacket) 
        {
            List<CreaturePacket> incomingCreatureList = syncAllCreaturesPacket.creatures.ToList<CreaturePacket>();
            for (int i = 0; incomingCreatureList.Count > i; i++)
            {
                int indexToReplace = MainManager.creatureList.FindIndex(c => c.instanceId == incomingCreatureList[i].creature.instanceId);
                if (indexToReplace != -1)
                {
                    int id = MainManager.creatureList[indexToReplace].instanceId;
                    try
                    {
                        Creature oldCreature = MainManager.creatureList[indexToReplace];
                        MainManager.creatureList[indexToReplace] = incomingCreatureList[i].creature;
                        MainManager.creatureList[indexToReplace].receivedPacketMove = true;
                        MainManager.creatureList[indexToReplace].creatureAI = oldCreature.creatureAI;
                        MainManager.creatureList[indexToReplace].creatureAI.creature = MainManager.creatureList[indexToReplace];
                    }
                    catch (Exception ex) { Printer.LogWarning(ex.ToString()); }
                }
                else
                {
                    CreaturePacket creaturePacket = incomingCreatureList[i];
                    MainManager.respawnManager.EntityRespawn(creaturePacket.creature, Converter.Vector3ToUnityVector3(creaturePacket.creature.stats.currentPosition));
                }
            }
            if(syncAllCreaturesPacket.deadCreatures.Count > 0) 
            {
                for (int i = 0; syncAllCreaturesPacket.deadCreatures.Count > i; i++)
                {
                    int id = syncAllCreaturesPacket.deadCreatures[i];
                    try
                    {
                        Creature creature = FindCreature(id);
                        creature.receivedPacketDeath = true;
                    }
                    catch (Exception ex)
                    {
                        Printer.LogWarning($"Tried killing creature {id}, but it was already dead.\n \n{ex}");
                    }
                }
            }
        }
        public static Creature FindCreature(int id) 
        {
            Creature creature = null;
            IEnumerable<Creature> matchingCreatures = MainManager.creatureList.Where(p => p.instanceId == id);
            if (matchingCreatures.Any()) 
            {
                creature = matchingCreatures.First();
            }
            return creature;
        }

        public static SyncAllPlayersPacket SyncPlayersWithClient(GameClient client)
        {
            SyncAllPlayersPacket packet = new SyncAllPlayersPacket();
            List<PlayerPacket> playerList = new List<PlayerPacket>();
            foreach (Player player in MainManager.playerList.ToList())
            {
                //if (creature.creatureAI.creature.stats.needsUpdating)
                //{
                PlayerPacket playerPacket = new PlayerPacket();
                playerPacket.player = player;
                playerList.Add(playerPacket);
                player.needsUpdating = false;

                //}
            }
            foreach (int i in deadPlayers)
            {
                if (!client.deadCreatureList.Contains(i))
                {
                    client.deadPlayersList.Add(i);
                    packet.deadPlayers.Add(i);
                }
            }
            packet.players = playerList.ToArray();
            return packet;
        }

        public static void SyncPlayerFromServer(SyncAllPlayersPacket syncAllPlayersPacket)
        {
            List<PlayerPacket> incomingPlayerList = syncAllPlayersPacket.players.ToList<PlayerPacket>();
            for (int i = 0; incomingPlayerList.Count > i; i++)
            {
                int indexToReplace = MainManager.playerList.FindIndex(c => c.id == incomingPlayerList[i].player.id);
                if (indexToReplace != -1)
                {
                    int id = MainManager.playerList[indexToReplace].id;
                    try
                    {
                        Creature oldCreature = MainManager.creatureList[indexToReplace];
                        MainManager.playerList[indexToReplace] = incomingPlayerList[i].player;
                        MainManager.playerList[indexToReplace].receivedPacketMove = true;
                    }
                    catch (Exception ex) { Printer.LogWarning(ex.ToString()); }
                }
                else
                {
                    PlayerPacket playerPacket = incomingPlayerList[i];
                    MainManager.respawnManager.PlayerRespawn(playerPacket.player);
                }
            }
        }
    }
}

