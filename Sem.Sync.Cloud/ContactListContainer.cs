// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactListContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The container structure to transport information between client and cloud
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Sem.Sync.SyncBase;

    /// <summary>
    /// The container structure to transport information between client and cloud
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactListContainer : ResultBase
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the list of StdContact elements.
        /// </summary>
        [DataMember]
        public List<StdContact> ContactList { get; set; }

        /// <summary>
        ///   Gets or sets the authentiaction credentials.
        /// </summary>
        [DataMember]
        public CloudCredentials Credentials { get; set; }

        #endregion
    }
}