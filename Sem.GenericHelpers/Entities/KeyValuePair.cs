//-----------------------------------------------------------------------
// <copyright file="KeyValuePair.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers.Entities
{
    /// <summary>
    /// A key value pair with string type for both members.
    /// List of KeyValuePair is needed for serialization and for generating
    /// Binding sources for UI.
    /// </summary>
    public class KeyValuePair
    {
        /// <summary>
        /// the "key" of the entry - there's no funcational difference to the value
        /// member of this type
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The "Value" of the entry - there's no funcational difference to the key
        /// member of this type
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="KeyValuePair"/> class.
        /// </summary>
        public KeyValuePair(){}
        
        /// <summary>
        /// Creates a new instance of the <see cref="KeyValuePair"/> class.
        /// </summary>
        public KeyValuePair(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}