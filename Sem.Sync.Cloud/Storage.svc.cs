// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Storage.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the service to read/write contacts to/from the blob storage
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System;
    using System.Collections.Generic;

    using CloudStorageConnector;

    using SyncBase;
    using SyncBase.Helpers;

    /// <summary>
    /// Implements the service to read/write contacts to/from the blob storage
    /// </summary>
    public class Storage : IStorage
    {
        /// <summary>
        /// Gets all contacts from the Azure storage with the specified Id.
        /// </summary>
        /// <param name="contactBlobId">The contacts id.</param>
        /// <returns>a list of <see cref="StdContact"/></returns>
        public ContactListContainer GetAll(string contactBlobId)
        {
            try
            {
                // this can be replaced by any class inheriting from StdClient
                var client = new BlobStorage();
                var stdContacts = new ContactListContainer
                {
                    ContactList = client.GetAll(contactBlobId ?? "default").ToContacts()
                };
                return stdContacts;
            }
            catch (Exception)
            {
                return new ContactListContainer { ContactList = new List<StdContact>() };
            }
        }

        /// <summary>
        /// Writes a list of contacts to the Azure storage
        /// </summary>
        /// <param name="elements"> The elements to write to the storage. </param>
        /// <param name="contactBlobId"> The contact blob id. </param>
        /// <param name="skipIfExisting"> This parameter is not used at the moment. </param>
        /// <returns> True if the operation was successful </returns>
        public bool WriteFullList(ContactListContainer elements, string contactBlobId, bool skipIfExisting)
        {
            try
            {
                // this can be replaced by any class inheriting from StdClient
                var client = new BlobStorage();
                client.WriteRange(elements.ContactList.ToStdElement(), contactBlobId ?? "default");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes a blob (= list of contacts)
        /// </summary>
        /// <param name="blobId"> The blob id. </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void DeleteBlob(string blobId)
        {
            throw new NotImplementedException();
        }
    }
}
