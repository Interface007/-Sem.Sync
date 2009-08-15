// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValuePair.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   A key value pair with string type for both members.
//   List of KeyValuePair is needed for serialization and for generating
//   Binding sources for UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        /// Initializes a new instance of the <see cref="KeyValuePair"/> class. 
        /// </summary>
        public KeyValuePair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuePair"/> class. 
        /// </summary>
        /// <param name="key"> The key of the new key value pair. </param>
        /// <param name="value"> The value of the new key value pair. </param>
        public KeyValuePair(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the "Key" of the entry - there's no funcational difference to the value
        /// member of this type
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the "Value" of the entry - there's no funcational difference to the key
        /// member of this type
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Overrides the to ToString method from object to present a meaningful string that described this instance
        /// </summary>
        /// <returns>a string with the key and the value</returns>
        public override string ToString()
        {
            return string.Format("key = '{0}', value = '{1}'", this.Key, this.Value);
        }
    }
}