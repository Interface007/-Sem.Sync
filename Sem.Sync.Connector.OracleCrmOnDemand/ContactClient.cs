// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OracleCrmOnDemand
{
    using System.Collections.Generic;

    using SyncBase;
    using SyncBase.Attributes;

    /// <summary>
    /// Implements a read/write connector to "Oracle CRM on Demand"
    /// </summary>
#if DEBUG
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(DisplayName = "Oracle CRM On Demand")]
#else
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(DisplayName = "Oracle CRM On Demand", Internal = true)]
#endif
    public class ContactClient : StdClient
    {
        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">The clientFolderName is currently ignored by the connector.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            CleanUpEntities(result);
            return result;
        }

        /// <summary>
        /// Writes the entries to the destination
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> The clientFolderName is currently ignored by the connector. </param>
        /// <param name="skipIfExisting"> A value indicating if existing entries should be skipped while writing. </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
        }
    }
}