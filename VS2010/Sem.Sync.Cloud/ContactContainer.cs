// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactContainer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   container class for one single contact
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    using Sem.Sync.SyncBase;

    /// <summary>
    /// container class for one single contact
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactContainer
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the contact on this container instance.
        /// </summary>
        [DataMember]
        public StdContact Contact { get; set; }

        #endregion
    }
}