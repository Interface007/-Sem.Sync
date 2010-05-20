// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleContactGroups.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements a group of contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Google
{
    using System;
    using System.Collections.Generic;

    using global::Google.Contacts;

    /// <summary>
    /// Implements a group of contacts
    /// </summary>
    public class GoogleContactGroups
    {
        #region Constants and Fields

        /// <summary>
        ///   A cache list for GetGroupByName
        /// </summary>
        private readonly Dictionary<string, Group> cache = new Dictionary<string, Group>();

        /// <summary>
        ///   The object responsible to interact with the service api
        /// </summary>
        private readonly ContactsRequest myRequester;

        /// <summary>
        ///   The user uri to authenticate
        /// </summary>
        private readonly Uri myUri;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleContactGroups"/> class.
        /// </summary>
        /// <param name="requester">
        /// The contact request object that acts as a requester.  
        /// </param>
        /// <param name="userUri">
        /// The user Uri. 
        /// </param>
        public GoogleContactGroups(ContactsRequest requester, Uri userUri)
        {
            this.myRequester = requester;
            this.myUri = userUri;
        }

        #endregion
    }
}