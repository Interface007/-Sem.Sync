// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudiVZContacts.cs" company="Sven Erik Matzen">
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
    /// This class is the client class for handling contacts persisted to the StudiVZ part of the
    /// social network providing the sited MeinVZ and StudiVZ.
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "StudiVZ",
        CanReadContacts = true,
        CanWriteContacts = false,
        NeedsCredentialsDomain = false, 
        MatchingIdentifier = ProfileIdentifierType.MeinVZ,
        NeedsCredentials = true)]
    public class StudiVzContacts : ContactClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudiVzContacts"/> class.
        /// </summary>
        public StudiVzContacts()
        {
            this.HttpDetectionStringLogOnFailed = "action=\"https://secure.studivz.net/Login\"";
            this.HttpUrlLogOnRequest = "https://secure.studivz.net/Login";
            this.HttpUrlBaseAddress = "http://www.studivz.net/";
            
            this.HttpRequester.BaseUrl = this.HttpUrlBaseAddress;
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "StudiVZ-Connector";
            }
        }
    }
}
