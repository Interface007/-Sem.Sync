// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OnlineStorage
{
    using System.Collections.Generic;
    using System.Linq;

    using ContactService2;

    using Sem.GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;

    /// <summary>
    /// Implements a sample client for the sample online storage (accessed via WCF).
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = false)]
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = true, NeedsCredentials = true, DisplayName = "SEM-Online sample")]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// Returns a human readable name of this class.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "OnlineStorage-Connector";
            }
        }

        /// <summary>
        /// Reads the full list of contacts from the online storage
        /// </summary>
        /// <param name="clientFolderName">represents a path to the data</param>
        /// <param name="result">the list that will be filled with the contacts</param>
        /// <returns>the list of contacts that has been read from the online storage</returns>
        protected override List<SyncBase.StdElement> ReadFullList(string clientFolderName, List<SyncBase.StdElement> result)
        {
            var client = new ContactServiceClient();
            var contacts = Tools.LoadFromString<List<StdContact>>(client.GetAll(clientFolderName, 1, 10).ContactList);
            result.AddRange(contacts);

            return result;
        }

        /// <summary>
        /// Writes the elements to the sample online store.
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> represents a path to the data </param>
        /// <param name="skipIfExisting"> If this parameter is true, existing elements will not be altered. </param>
        protected override void WriteFullList(List<SyncBase.StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var client = new ContactService2.ContactServiceClient();
            client.WriteFullList(
                new ContactListContainer
                    {
                        ContactList = Tools.SaveToString(elements)
                    },
                    clientFolderName,
                    skipIfExisting);
        }
    }
}
