// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableDictionary.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the SerializableDictionary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements a serializable dictionary.
    /// </summary>
    /// <typeparam name="TKey">the type of the key of the dictionary entries</typeparam>
    /// <typeparam name="TValue">the type of the values of the dictionary entries</typeparam>
    [XmlRoot("dictionary")]
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey,TValue}"/> class. 
        /// </summary>
        public SerializableDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey,TValue}"/> class. 
        /// </summary>
        /// <param name="init">The data this dictionary should get.</param>
        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> init)
        {
            init.ForEach(x => this.Add(x.Key, x.Value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="info"> The information to write the data. </param>
        /// <param name="context"> The streaming context for serialization. </param>
        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The schema of the serialized dictionary - returns NULL
        /// </summary>
        /// <returns> returns alway NULL </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Deserialization of the dicrionary,
        /// </summary>
        /// <param name="reader"> The reader that has access to the xml. </param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                if (reader.LocalName != "item")
                {
                    if (typeof(TKey).BaseType == typeof(Enum))
                    {
                        var keyName = this.TranslateKey(reader.LocalName);
                        var elementContent = reader.ReadElementString();
                        var keyValue = this.CreateNewValueItem(elementContent);
                        this.Add((TKey)Enum.Parse(typeof(TKey), keyName), keyValue);
                        continue;
                    }
                }

                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                var key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                var value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Serialization implementation of the dictionary.
        /// </summary>
        /// <param name="writer"> The serialization writer. </param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                var value = this[key];
                writer.WriteStartElement("value");
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Implementation of the dictionary functionality.
        /// </summary>
        /// <param name="key"> The key of the value to return. </param>
        /// <returns>The value if the key exists, otherwise the default value of the value's Type <typeparamref name="TValue"/> </returns>
        public TValue GetValue(TKey key)
        {
            return this.ContainsKey(key) ? this[key] : default(TValue);
        }

        /// <summary>
        /// Translation hook vor inherited dictionaries to support legacy structures.
        /// </summary>
        /// <param name="keyName"> The key name. </param>
        /// <returns> The translated key name. </returns>
        protected virtual string TranslateKey(string keyName)
        {
            return keyName;
        }

        /// <summary>
        /// Deserializes a value of the dictionary.
        /// </summary>
        /// <param name="value"> The value to be deserialized as an xml-string. </param>
        /// <returns>The deserialized value.</returns>
        protected virtual TValue CreateNewValueItem(string value)
        {
            var valueSerializer = new XmlSerializer(typeof(TValue));
            return (TValue)valueSerializer.Deserialize(XDocument.Parse(value).CreateReader());
        }
    }
}
