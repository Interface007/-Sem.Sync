// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdProfileInformationClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   implements a standard profile information client that is capable to read/write profile information.
//   This class is originally intended to serve as a base class for clients that enable editing of
//   social network profiles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using Sem.GenericHelpers.Interfaces;

    /// <summary>
    /// implements a standard profile information client that is capable to read/write profile information.
    ///   This class is originally intended to serve as a base class for clients that enable editing of
    ///   social network profiles.
    /// </summary>
    public abstract class StdProfileInformationClient : ICredentialAware
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a domain to log on. In a windows domain this is the domain name, 
        ///   in case of a web form this may be omitted. Most client implementation do accept
        ///   specifying the domain also as part of the user name: domain\user or user@domain.
        /// </summary>
        public string LogOnDomain { get; set; }

        /// <summary>
        ///   Gets or sets the password to log on. Be aware that this information is READ/WRITE. This
        ///   may be a security issue for your user!
        /// </summary>
        public string LogOnPassword { get; set; }

        /// <summary>
        ///   Gets or sets a user name to log on. Most client implementation do accept
        ///   specifying the domain also as part of the user name: domain\user or user@domain.
        /// </summary>
        public string LogOnUserId { get; set; }

        #endregion
    }
}