using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using static UnityEditor.Progress;

namespace Shared 
{
    public static class CreatureManager
    {
        static public Dictionary<string, GameObject> creatureObjectDic = new Dictionary<string,GameObject>();
        static public List<Creature> creatureList = new List<Creature>();
        static CreatureManager()
        {
            LoadAllCreatures();
            LoadAllCreatureObjects();
        }
        public static void LoadAllCreatures() 
        {
            TextAsset[] itemObjects = Resources.LoadAll<TextAsset>("JsonData/Creatures");
            for (int i = 0; i < itemObjects.Length; i++)
            {
                string converter = itemObjects[i].text;
                creatureList.Add(JsonUtility.FromJson<Creature>(converter));
            }
        }

        public static void LoadAllCreatureObjects()
        {
            List<GameObject> creatureObjects = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures").ToList();
            for (int i = 0; i < creatureList.Count; ++i)
            {
                for (int j = 0; j < creatureObjects.Count(); ++j)
                {
                    if (creatureObjects[j].name == creatureList[i].uniqueName)
                    {
                        creatureObjectDic.Add(creatureList[i].uniqueName, creatureObjects[j]);
                    }
                }
            }
        }

        public static GameObject FindPrefab(string uniqueName) 
        {
            foreach (string name in creatureObjectDic.Keys)
            {
                if(name == uniqueName) 
                {
                    return creatureObjectDic[name];
                }
            }
            return null;
        }

        public static Creature CreateCreature(Creature baseCreature) 
        {
            Printer.Log(baseCreature.stats.hitPoint.ToString());
            string baseStats = JsonUtility.ToJson(baseCreature);
            Creature creature = JsonUtility.FromJson<Creature>(baseStats);
            Printer.Log(creature.stats.hitPoint.ToString());
            return creature;
        }
    }
}