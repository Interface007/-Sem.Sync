// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableDictionary.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The serializable dictionary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// The serializable dictionary.
    /// </summary>
    /// <typeparam name="TKey">
    /// </typeparam>
    /// <typeparam name="TValue">
    /// </typeparam>
    [XmlRoot("dictionary")]
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Implemented Interfaces

        #region IXmlSerializable

        /// <summary>
        /// The get schema.
        /// </summary>
        /// <returns>
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// The read xml.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        public void ReadXml(XmlReader reader)
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
        /// The write xml.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void WriteXml(XmlWriter writer)
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

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The create new value item.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual TValue CreateNewValueItem(string value)
        {
            var valueSerializer = new XmlSerializer(typeof(TValue));
            return (TValue)valueSerializer.Deserialize(XDocument.Parse(value).CreateReader());
        }

        /// <summary>
        /// The translate key.
        /// </summary>
        /// <param name="keyName">
        /// The key name.
        /// </param>
        /// <returns>
        /// The translate key.
        /// </returns>
        protected virtual string TranslateKey(string keyName)
        {
            return keyName;
        }

        #endregion
    }
}