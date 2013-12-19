// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudCredentials.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Stores credential information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Stores credential information
    /// </summary>
    [DataContract]
    public class CloudCredentials
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the domain string of the account.
        /// </summary>
        [DataMember]
        public string AccountDomain { get; set; }

        /// <summary>
        ///   Gets or sets the ID of the account.
        /// </summary>
        [DataMember]
        public string AccountId { get; set; }

        /// <summary>
        ///   Gets or sets the password of the account.
        /// </summary>
        [DataMember]
        public string AccountPassword { get; set; }

        #endregion
    }
}