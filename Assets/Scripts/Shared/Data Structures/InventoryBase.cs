using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared 
{
    [Serializable]
    public class Inventory 
    { 
        public List<Item> content = new List<Item>();
        public int id;
    }
}