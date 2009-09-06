// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StudiVZ.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.MeinVZ
{
    #region usings

    using SyncBase.Attributes;
    using SyncBase.DetailData;
    
    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "StudiVZ",
        CanRead = true,
        CanWrite = false,
        MatchingIdentifier = ProfileIdentifierType.MeinVZ,
        NeedsCredentials = true)]
    public class StudiVZ : ContactClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudiVZ"/> class.
        /// </summary>
        public StudiVZ()
        {
            this.HttpDetectionStringLogonFailed = "action=\"https://secure.studivz.net/Login\"";
            this.HttpUrlLogonRequest = "https://secure.studivz.net/Login";
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
                return "MeinVZ-Connector";
            }
        }
    }
}
