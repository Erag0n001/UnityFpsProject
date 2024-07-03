using Shared;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
namespace Shared
{
    public static class Serializer
    {
        private static JsonSerializerSettings DefaultSettings => new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None };
        public static T ConvertBytesToObject<T>(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream();

            JsonSerializer serializer = JsonSerializer.Create(DefaultSettings);

            using (BsonReader reader = new BsonReader(memoryStream))
            {
                return serializer.Deserialize<T>(reader);
            }
            
        }

        //Serialize from and to packets

        public static byte[] ConvertObjectToBytes(object toConvert)
        {
            if (toConvert == null) return null;

            MemoryStream memoryStream = new MemoryStream();

            JsonSerializer serializer = JsonSerializer.Create(DefaultSettings);

            using (BsonWriter writer = new BsonWriter(memoryStream))
            {
                serializer.Serialize(writer, toConvert);
            }

            return memoryStream.ToArray();
        }
        //Serialize from and to strings

        public static string SerializeToString(object serializable)
        {
            return JsonConvert.SerializeObject(serializable);
        }

        public static T SerializeFromString<T>(string serializable)
        {
            return JsonConvert.DeserializeObject<T>(serializable);
        }
    }
}