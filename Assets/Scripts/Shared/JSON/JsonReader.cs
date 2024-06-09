using Newtonsoft.Json;
using System;
using System.IO;
using Unity.VisualScripting;

namespace Shared 
{ 
    static class JsonReader 
    {
        public static T ReadDirectory<T>() 
        {
            //foreach (var file in dir.GetFiles("*.json"))
            //{
            //    return JsonConvert.DeserializeObject<T>(File.ReadAllText(file.FullName));
            //}
            return default(T);
        }
    }
}