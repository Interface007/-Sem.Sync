// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Serializer.cs" company="Sven Erik Matzen">
//   Copyright (c) SDX-AG
// </copyright>
// <summary>
//   a serializer class to serialize and deserialize an entity to/from binary data - this class has been developed at 
//   SDX-AG for implementing a sample Azure application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.CloudStorage.Helper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using SyncBase;

    /// <summary>
    /// a serializer class to serialize and deserialize an entity to/from binary data.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes an entity to binary data.
        /// </summary>
        /// <typeparam name="T">the type of entities</typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns>The List of Entites as a Byte Array</returns>
        public static byte[] SerializeBinary<T>(T entities) 
        {
            var serializer = new BinaryFormatter();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, entities);
            return memStream.ToArray();
        }

        /// <summary>
        /// Deserializes the serialized byte array.
        /// </summary>
        /// <typeparam name="T">The type of the entities</typeparam>
        /// <param name="serializedEntities">The serialized entities.</param>
        /// <returns>a list of deserialized entities</returns>
        public static T DeSerializeBinary<T>(byte[] serializedEntities) where T : class
        {
            var stream = new MemoryStream(serializedEntities) { Position = 0 };
            var deserializer = new BinaryFormatter();
            var newobj = deserializer.Deserialize(stream);
            stream.Close();
            return newobj as T;
        }
    }
}