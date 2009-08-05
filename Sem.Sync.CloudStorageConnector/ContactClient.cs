// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.Sync.CloudStorageConnector.CloudStorage;

namespace Sem.Sync.CloudStorageConnector
{
    #region usings

    using System.Collections.Generic;
    using System.Xml.Serialization;

    using StorageConnectors;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to a cloud storage system.
    /// The interface <see cref="IStorageConnector"/> does provide definition for the functionality
    /// the storage class must provide.
    /// </summary>
    [ClientStoragePathDescriptionAttribute(
        Mandatory = true,
        Default = "{FS:WorkingFolder}\\Public",
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    [ConnectorDescription(DisplayName = "Cloud Contact Connector to be used inside the cloud")]
    public class ContactClient : StdClient
    {

        private readonly IStorage _storage = new CloudStorage.StorageClient();

        /// <summary>
        /// This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(List<StdContact>));

        /// <summary>
        /// Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Cloud Contact Connector";
            }
        }



        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and
        /// overwriting the elements if they do already exist. Missing elements will be added, existing
        /// elements will overwritten with the new elements.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside -
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public override void WriteRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string blobId, List<StdElement> result)
        {
            var contacts = new List<StdElement>();
            var container = _storage.GetAll(blobId);
            foreach(var elem in container.ContactList)
            {
                contacts.Add(elem);
            }
            return contacts;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="blobId">the full name including path of the file that will get the contacts while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string blobId, bool skipIfExisting)
        {
            ContactListContainer container = new ContactListContainer();
            container.ContactList = new StdContact[elements.Count];
            for(int i = 0; i < elements.Count; i++)
            {
                container.ContactList[i] = elements[i] as StdContact;
            }
            _storage.WriteFullList(container, blobId, skipIfExisting);
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        /// <param name="blobId">The BLOB id.</param>
        /// <returns></returns>
        public List<StdElement> GetContacts(string blobId)
        {
            return ReadFullList(blobId, null);
        }

        /// <summary>
        /// Deletes the contacts.
        /// </summary>
        /// <param name="blobId">The BLOB id.</param>
        public void DeleteContacts(string blobId)
        {
            _storage.DeleteBlob(blobId);
        }

    }
}
