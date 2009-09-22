// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobStorageManager.cs" company="SDX-AG">
//   Copyright (c) SDX-AG.
// </copyright>
// <summary>
//   Manager Class for handling Blobs - this class has been developed at SDX-AG for implementing a sample Azure application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.Connector.CloudStorage.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Samples.ServiceHosting.StorageClient;

    using SyncBase;

    /// <summary>
    /// Manager Class for handling Blobs
    /// </summary>
    public class BlobStorageManager
    {
        #region Fields
        /// <summary>
        /// Accountinfo for the message queue.
        /// </summary>
        private readonly StorageAccountInfo accountInfo;

        /// <summary>
        /// Contains the queueStorage instance.
        /// </summary>
        private readonly BlobStorage blobStorage;

        /// <summary>
        /// The BlobContainer
        /// </summary>
        private readonly BlobContainer blobContainer;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageManager"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        public BlobStorageManager(string containerName)
        {
            this.accountInfo = StorageAccountInfo.GetDefaultBlobStorageAccountFromConfiguration();
            this.blobStorage = BlobStorage.Create(this.accountInfo);

            this.blobContainer = this.blobStorage.GetBlobContainer(containerName);

            // Create the container if it does not exist.
            this.blobContainer.CreateContainer(null, ContainerAccessControl.Public);
        }

        /// <summary>
        /// Adds or updates a BLOB.
        /// </summary>
        /// <typeparam name="T">the type of the entity to add or update</typeparam>
        /// <param name="entities">The List of Entities.</param>
        /// <param name="contactBlobId">The scenario id.</param>
        public void AddOrUpdateBlob<T>(List<T> entities, string contactBlobId) where T : StdContact
        {
            this.blobContainer.CreateBlob(
                new BlobProperties(contactBlobId), 
                new BlobContents(Serializer.SerializeBinary(entities)), 
                true);
        }

        /// <summary>
        /// Deletes the BLOB with the scenarioId.
        /// </summary>
        /// <param name="contactBlobId">The scenario id.</param>
        public void DeleteBlob(string contactBlobId)
        {
            this.blobContainer.DeleteBlob(contactBlobId);
        }

        /// <summary>
        /// Gets the List of Entities from BLOB.
        /// </summary>
        /// <typeparam name="T">the type of the entity to read from the BLOB</typeparam>
        /// <param name="contactsId">The scenario id.</param>
        /// <returns>a list of entities</returns>
        public List<T> GetEntitiesFromBlob<T>(string contactsId) where T : StdContact
        {
            if (contactsId == null)
            {
                throw new ArgumentNullException("contactsId");
            }

            var contents = new BlobContents(new MemoryStream());
            if (this.blobContainer.DoesBlobExist(contactsId))
            {
                this.blobContainer.GetBlob(contactsId, contents, false);
                return Serializer.DeSerializeBinary<T>(contents.AsBytes());
            }

            return new List<T>();
        }
        #endregion
    }
}