using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using Sem.Sync.SyncBase;

namespace Sem.Sync.Cloud
{
    class Serializer
    {
        /// <summary>
        /// Serializes an Entity to Binarydata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns>The List of Entites as a Byte Array</returns>
        public static Byte[] SerializeBinary<T>(List<T> entities) where T : StdContact
        {
            BinaryFormatter serializer = new BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, entities);
            return memStream.ToArray();
        }

        /// <summary>
        /// Deserializes the serialized byte array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedEntities">The serialized entities.</param>
        /// <returns></returns>
        public static List<T> DeSerializeBinary<T>(Byte[] serializedEntities) where T : StdContact
        {
            MemoryStream stream = new MemoryStream(serializedEntities);
            stream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newobj = deserializer.Deserialize(stream);
            stream.Close();
            return newobj as List<T>;
        }

    }
}
