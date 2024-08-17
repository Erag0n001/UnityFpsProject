using System.Linq;
using System.Numerics;
using UnityEngine;

namespace Shared 
{
    public static class Converter 
    {
        public static SerializableVector4 QuaternionToVector4(UnityEngine.Quaternion q) 
        {
            return new SerializableVector4() 
            {
                w = q.w,
                x = q.x,
                y = q.y,
                z = q.z,
            };
        }
        public static UnityEngine.Quaternion Vector4ToQuaternion(SerializableVector4 v) 
        {
            return new UnityEngine.Quaternion() 
            {
                w = v.w,
                x = v.x,
                y = v.y,
                z = v.z,
            };
        }
        public static SerializableVector3 Vector3UnityToVector3(UnityEngine.Vector3 v)
        {
            return new SerializableVector3()
            {
                x = v.x,
                y = v.y,
                z = v.z,
            };
        }

        public static UnityEngine.Vector3 Vector3ToUnityVector3(SerializableVector3 v) 
        {
            return new UnityEngine.Vector3()
            {
                x = v.x,
                y = v.y,
                z = v.z,
            };
        }
        static public Inventory SerializableInventoryToInventory(SerializableInventory serializableInventory)
        {
            return new Inventory()
            {
                content = serializableInventory.itemList.ToList<Item>(),
                id = serializableInventory.id
            };
        }
    }
}