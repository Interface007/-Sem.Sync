﻿// --------------------------------------------------------------------------------------------------------------------
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

    using Azure.Storage;

    using SyncBase;
    using SyncBase.Helpers;

    using BlobStorage = Connector.CloudStorage.BlobStorage;

    /// <summary>
    /// Implements the service to read/write contacts to/from the blob storage
    /// </summary>
    public class Storage : IStorage
    {
        /// <summary>
        /// Gets all contacts from the Azure storage with the specified Id.
        /// </summary>
        /// <param name="blobId"> The blob Id. </param>
        /// <returns> a list of <see cref="StdContact"/> </returns>
        public ContactListContainer GetAll(string blobId)
        {
            AccessControlHelper.DemandActionClaim("Contacts.GetAll");
            try
            {
                // this can be replaced by any class inheriting from StdClient
                var client = new BlobStorage();
                var stdContacts = new ContactListContainer
                {
                    ContactList = client.GetAll(blobId ?? "default").ToContacts()
                };
                return stdContacts;
            }
            catch (Exception ex)
            {
                return 
                    new ContactListContainer
                        {
                            ContactList = new List<StdContact>(),
                            Messages = new List<TechnicalMessage>
                                {
                                    new TechnicalMessage
                                        {
                                            Message = ex.Message
                                        }
                                }
                        };
            }
        }

        /// <summary>
        /// Writes a list of contacts to the Azure storage
        /// </summary>
        /// <param name="elements"> The elements to write to the storage. </param>
        /// <param name="blobId"> The contact blob id. </param>
        /// <param name="skipIfExisting"> This parameter is not used at the moment. </param>
        /// <returns> True if the operation was successful </returns>
        public BooleanResultContainer WriteFullList(ContactListContainer elements, string blobId, bool skipIfExisting)
        {
            AccessControlHelper.DemandActionClaim("Contacts.WriteAll");
            try
            {
                // this can be replaced by any class inheriting from StdClient
                var client = new BlobStorage();
                client.WriteRange(elements.ContactList.ToStdElement(), blobId ?? "default");
            }
            catch (Exception ex)
            {
                return
                    new BooleanResultContainer
                    {
                        Result = false,
                        Messages = new List<TechnicalMessage>
                                {
                                    new TechnicalMessage
                                        {
                                            Message = ex.Message
                                        }
                                }
                    };
            }

            return new BooleanResultContainer
                    {
                        Result = true,
                    };
        }

        /// <summary>
        /// Deletes a blob (= list of contacts). This method is currently not implemented, so it
        /// does return a technical message "Method not implemented".
        /// </summary>
        /// <param name="blobId"> The blob id.  </param>
        /// <returns> A value indicating wherther the delete process was successfull. This result container 
        /// may also contain technical messages. </returns>
        public BooleanResultContainer DeleteBlob(string blobId)
        {
            AccessControlHelper.DemandActionClaim("Contacts.DeleteAll");
            return new BooleanResultContainer
                {
                    Messages = new List<TechnicalMessage>
                    { 
                        new TechnicalMessage
                        {
                            Classification = MessageClassification.Error,
                            Message = "Method not implemented",
                            MessageId = 1
                        }
                    }
                };
        }
    }
}