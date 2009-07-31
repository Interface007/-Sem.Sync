using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Samples.ServiceHosting.StorageClient;
using System.IO;
using Sem.Sync.SyncBase;

namespace Sem.Sync.Cloud
{
    /// <summary>
    /// Manager Class for handling Blobs
    /// </summary>
    public class BlobStorageManager
    {
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
            //Create the container if it does not exist.
            this.blobContainer.CreateContainer(null, ContainerAccessControl.Public);
        }

        /// <summary>
        /// Adds or updates a BLOB.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The List of Entities.</param>
        /// <param name="contactBlobId">The scenario id.</param>
        public void AddOrUpdateBlob<T>(List<T> entities, string contactBlobId) where T : StdContact
        {
            this.blobContainer.CreateBlob(new BlobProperties(contactBlobId),
                                     new BlobContents(Serializer.SerializeBinary(entities)), true);

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
        /// <typeparam name="T"></typeparam>
        /// <param name="contactsId">The scenario id.</param>
        /// <returns></returns>
        public List<T> GetEntitiesFromBlob<T>(string contactsId) where T : StdContact
        {
            BlobContents contents = new BlobContents(new MemoryStream());
            if (this.blobContainer.DoesBlobExist(contactsId))
            {
                this.blobContainer.GetBlob(contactsId, contents, false);
                return Serializer.DeSerializeBinary<T>(contents.AsBytes());
            }
            return null;

        }
        #endregion

        #region Fields
        /// <summary>
        /// Accountinfo for the message queue.
        /// </summary>
        private StorageAccountInfo accountInfo;
        /// <summary>
        /// Contains the queueStorage instance.
        /// </summary>
        private BlobStorage blobStorage;
        /// <summary>
        /// The BlobContainer
        /// </summary>
        private BlobContainer blobContainer;
        #endregion
    }
}
