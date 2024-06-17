using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared 
{
    public static class CreatureLoader
    {
        static public Dictionary<string,List<GameObject>> creatureObjectDic = new Dictionary<string,List<GameObject>>();
        static public List<Creature> creatureList = new List<Creature>();
        static CreatureLoader()
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
                int type = (int)JsonUtility.FromJson<ItemType>(converter).itemType;
                creatureList.Add(JsonUtility.FromJson<Creature>(converter));
            }
        }

        public static void LoadAllCreatureObjects()
        {
            List<GameObject> aggressiveList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Aggressive").ToList();
            List<GameObject> neutralList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Neutral").ToList();
            List<GameObject> passiveList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Passive").ToList();
        }
    }
}