using Shared;
using UnityEditor;
using UnityEngine;

namespace Client 
{
    class UnityEditorAssetCreator
    {
        //Unity editor Asset Creator
        [MenuItem("Assets/Create/JsonData/ItemBase/Item")]
        private static void CreateNewItem()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "Item.json",
                JsonUtility.ToJson(new Item(), true));
        }
        [MenuItem("Assets/Create/JsonData/ItemBase/RangedWeapon")]
        private static void CreateNewRangedWeapon()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "RangedWeapon.json",
                JsonUtility.ToJson(new RangedWeapon(), true));
        }
        [MenuItem("Assets/Create/JsonData/ItemBase/RangedAmmo")]
        private static void CreateNewRangedAmmo()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "RangedAmmo.json",
                JsonUtility.ToJson(new Ammo(), true));
        }
        [MenuItem("Assets/Create/JsonData/Creature")]
        private static void CreateNewCreature()
        {
            ProjectWindowUtil.CreateAssetWithContent(
                "CreatureStats.json",
                JsonUtility.ToJson(new CreatureBase(), true));
        }
    }
}