using System;
using Unity.VisualScripting;

namespace Shared 
{
    [Serializable]
    public class SerializableVector3 
    {
        public SerializableVector3(float x = 0, float y = 0, float z = 0) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public float x;
        public float y;
        public float z;
    }
    [Serializable]
    public class SerializableVector4
    {
        public SerializableVector4(float x = 0, float y = 0, float z = 0, float w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public float x;
        public float y;
        public float z;
        public float w;
    }
    [Serializable]
    public class SerializableInventory 
    {
        public SerializableInventory(Inventory inventory)
        {
            if(inventory == null) 
            {
                this.itemList = null;
                this.id = 0;
            } else 
            {
                this.itemList = inventory.content.ToArray();
                this.id = inventory.id;
            }
        }
        public Item[] itemList;
        public int id;
    }
}