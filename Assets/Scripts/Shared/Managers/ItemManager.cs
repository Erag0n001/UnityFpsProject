using System.Collections.Generic;
using UnityEngine;
namespace Shared
{
    public static class ItemManager
    {
        public static Dictionary<string, Sprite> textureDic = new Dictionary<string, Sprite>();
        public static List<Item> objectList = new List<Item>();
        static ItemManager() 
        {
            LoadAllItems();
            LoadAllTextures();
            Printer.Log("Loaded all Items");
        }

        public static Item FetchItem()
        {
            return CreateNewItem(objectList[0]);
        }

        public static Item CreateNewItem(Item itemToAssign)
        {
            string baseStats = JsonUtility.ToJson(itemToAssign);
            Item newItem = JsonUtility.FromJson<Item>(baseStats);
            return newItem;
        }

        public static Item CreateNewEmptyItem(Item itemToAssign)
        {
            string baseStats = JsonUtility.ToJson(itemToAssign);
            Item newItem = JsonUtility.FromJson<Item>(baseStats);
            newItem.amount = 0;
            return newItem;
        }

        public static void LoadAllItems()
        {
            TextAsset[] itemObjects = Resources.LoadAll<TextAsset>("JsonData/Items");
            for (int i = 0; i < itemObjects.Length; i++)
            {
                string converter = itemObjects[i].text;
                int type = (int)JsonUtility.FromJson<ItemType>(converter).itemType;
                switch (type)
                {
                    case 0:
                        objectList.Add(JsonUtility.FromJson<Item>(converter));
                        break;
                    case 1:
                        objectList.Add(JsonUtility.FromJson<RangedWeapon>(converter));
                        break;
                    case 2:
                        objectList.Add(JsonUtility.FromJson<Ammo>(converter));
                        break;
                    default:
                        Printer.LogError($"{itemObjects[i].name} was not able to find a match. This shouldn't happen\nThe following type was detected for the item: {type}");
                        break;
                }
            }
        }

        public static void LoadAllTextures()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/UI/Items");
            for (int i = 0; i < objectList.Count; ++i)
            {
                for (int j = 0; j < sprites.Length; ++j)
                {
                    if (sprites[j].name == objectList[i].iD)
                    {
                        textureDic.Add(objectList[i].iD, sprites[j]);
                    }
                }
            }
        }
    }
}