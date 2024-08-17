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
                    creature.creatureAI.creature.stats.needsUpdating = false;
                    
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
        public static void SyncAllCreaturesFromServer(SyncAllCreaturesPacket syncAllCreaturesPacket) 
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
                        MainManager.creatureList[indexToReplace].stats.receivedPacketMove = true;
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
                        creature.stats.receivedPacketDeath = true;
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
    }
}

