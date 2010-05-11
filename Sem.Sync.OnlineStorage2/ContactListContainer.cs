// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactListContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   a data contract class that contains one or more contacts in a list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage2
{
    using System.Runtime.Serialization;

    /// <summary>
    /// a data contract class that contains one or more contacts in a list.
    /// </summary>
    [DataContract]
    public class ContactListContainer
    {
        /// <summary>
        /// Gets or sets the list of contacts.
        /// </summary>
        [DataMember]
        public string ContactList { get; set; }

        [DataMember]
        public int TotalElements { get; set; }

        [DataMember]
        public int FirstElementIndex { get; set; }
    }
}