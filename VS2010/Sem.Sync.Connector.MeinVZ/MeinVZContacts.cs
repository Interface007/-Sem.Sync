﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeinVZContacts.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MeinVZ
{
    #region usings

    using SyncBase.Attributes;
    using SyncBase.DetailData;
    
    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the MeinVZ part of the
    /// social network providing the sited MeinVZ and StudiVZ.
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "MeinVZ",
        CanReadContacts = true,
        CanWriteContacts = false,
        NeedsCredentialsDomain = false,
        MatchingIdentifier = ProfileIdentifierType.MeinVZ,
        NeedsCredentials = true)]
    public class MeinVZContacts : ContactClient
    {
        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "MeinVZ-Connector";
            }
        }
    }
}
