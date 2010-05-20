// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactListContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   a data contract class that contains one or more contacts in a list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Sem.Sync.SyncBase;

    /// <summary>
    /// a data contract class that contains one or more contacts in a list.
    /// </summary>
    [DataContract]
    public class ContactListContainer
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the list of contacts.
        /// </summary>
        [DataMember]
        public List<StdContact> ContactList { get; set; }

        #endregion
    }
}