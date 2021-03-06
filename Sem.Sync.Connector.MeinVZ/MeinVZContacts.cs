﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeinVZContacts.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts persisted to the MeinVZ part of the
//   social network providing the sited MeinVZ and StudiVZ.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MeinVZ
{
    #region usings

    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the MeinVZ part of the
    ///   social network providing the sited MeinVZ and StudiVZ.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true, ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "MeinVZ", CanReadContacts = true, CanWriteContacts = false, 
        NeedsCredentialsDomain = false, MatchingIdentifier = ProfileIdentifierType.MeinVZ, NeedsCredentials = true)]
    public class MeinVzContacts : ContactClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StudiVzContacts" /> class.
        /// </summary>
        public MeinVzContacts()
        {
            this.HttpDetectionStringLogOnFailed = "action=\"https://secure.meinvz.net/Login\"";
            this.HttpUrlLogOnRequest = "https://secure.meinvz.net/Login";
            this.HttpUrlBaseAddress = "http://www.meinvz.net";
            
            this.HttpRequester.BaseUrl = this.HttpUrlBaseAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "MeinVZ-Connector";
            }
        }

        #endregion
    }
}