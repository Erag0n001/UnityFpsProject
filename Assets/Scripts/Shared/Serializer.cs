using Shared;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Newtonsoft.Json;
namespace Shared
{
    public static class Serializer
    {
        public static object ConvertBytesToObject(byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream();

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return binaryFormatter.Deserialize(memoryStream);
        }

        //Serialize from and to packets

        public static byte[] ConvertObjectToBytes(object toConvert)
        {
            if (toConvert == null) return null;

            MemoryStream memoryStream = new MemoryStream();

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, toConvert);
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