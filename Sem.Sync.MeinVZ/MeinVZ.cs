// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeinVZ.cs" company="Sven Erik Matzen">
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
    [ConnectorDescription(DisplayName = "MeinVZ",
        CanRead = true,
        CanWrite = false,
        MatchingIdentifier = ProfileIdentifierType.MeinVZ,
        NeedsCredentials = true)]
    public class MeinVZ : ContactClient
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
