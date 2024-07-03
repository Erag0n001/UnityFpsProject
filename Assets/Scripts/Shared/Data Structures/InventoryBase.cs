using System;
using System.Collections.Generic;

namespace Shared 
{
    [Serializable]
    public class Inventory 
    { 
        public List<Item> content = new List<Item>();
        public int id;
    }
}