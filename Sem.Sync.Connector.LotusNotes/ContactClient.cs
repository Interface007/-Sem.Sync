// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts persisted to the file system
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.LotusNotes
{
    using System;
    using System.Collections.Generic;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(Mandatory = true, ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "Lotus Notes")]
    public class ContactClient : StdClient
    {
        #region Methods

        /// <summary>
        /// Overrides virtual read method for full list of elements
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            // TODO: implement reading from the Lotus Notes server and map the entities to StdContact instances
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrides virtual write method for full list of elements
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// the information to where inside the source the elements should be written - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting">
        /// specifies whether existing elements should be updated or simply left as they are
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            // TODO: implement writing to the Lotus Notes server
            // HINT: To follow best practice, you should try to read each item by using 
            // the property path PersonalProfileIdentifiers.LotusNotesId as a filter
            // to query the Notes server, then update the fields and write back the
            // entry. You should NEVER delete entries to perform an update, because
            // you would loose properties not covered by StdContact.
            throw new NotImplementedException();
        }

        #endregion
    }
}