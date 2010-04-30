// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type accessing the Microsoft Azure implementation of an information store.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.OnlineStorage
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Text.RegularExpressions;

    using Cloud;
    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Helpers;

    /// <summary>
    /// Implements a sample client for the sample cloud storage (accessed via WCF).
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = false)]
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = true, NeedsCredentials = true, DisplayName = "Cloud Connector")]
    public class CloudClient : StdClient
    {
        /// <summary>
        /// Gets or sets the binding address for the storage client.
        /// </summary>
        public string BindingAddress { get; set; }

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
        /// Reads the full list of contacts from the cloud storage
        /// </summary>
        /// <param name="clientFolderName">represents a path to the data</param>
        /// <param name="result">the list that will be filled with the contacts</param>
        /// <returns>the list of contacts that has been read from the online storage</returns>
        protected override List<Sem.Sync.SyncBase.StdElement> ReadFullList(string clientFolderName, List<Sem.Sync.SyncBase.StdElement> result)
        {
            var client = this.GetClient();
            var contacts = client.GetAll(clientFolderName).ContactList;
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    result.Add(contact.ToStdElementBase());
                }
            }

            return result;
        }

        /// <summary>
        /// Writes the elements to the sample cloud store.
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> represents a path to the data </param>
        /// <param name="skipIfExisting"> If this parameter is true, existing elements will not be altered. </param>
        protected override void WriteFullList(List<Sem.Sync.SyncBase.StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            if (Regex.IsMatch(clientFolderName, "^http(s)?://.*(\\?)?"))
            {
                this.BindingAddress = clientFolderName.Split('?')[0];
                clientFolderName = clientFolderName.Split('?')[1];
            }

            var client = this.GetClient();
            var container = new ContactListContainer
                {
                    ContactList = elements.ToStdContactsCloud().ToArray(),
                    Credentials = new CloudCredentials
                        {
                            AccountId = this.LogOnUserId,
                            AccountPassword = this.LogOnPassword,
                            AccountDomain = this.LogOnDomain
                        }
                };

            client.WriteFullList(
                container,
                clientFolderName,
                skipIfExisting);
        }

        /// <summary>
        /// Creates a new storage client - does regard the <see cref="BindingAddress"/> property of this class.
        /// </summary>
        /// <returns> a new client for accessing the storage </returns>
        private StorageClient GetClient()
        {
            var client =
                string.IsNullOrEmpty(this.BindingAddress)
                ? new StorageClient()
                : new StorageClient(new BasicHttpBinding(BasicHttpSecurityMode.None), new EndpointAddress(this.BindingAddress));

            return client;
        }
    }
}
