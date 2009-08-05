using System;

namespace Sem.Sync.Cloud
{
    using Sem.Sync.SyncBase;

    // NOTE: If you change the class name "Storage" here, you must also update the reference to "Storage" in Web.config.
    public class Storage : IStorage
    {
        /// <summary>
        /// The file system path to store the information.
        /// </summary>
       // private readonly string _storagePath = "DefaultStorage"; // "C:\\ContactsServerData\\Contacts.xml";

        private readonly string _containerName = "contactcontainer";


        /// <summary>
        /// Gets all Contacts from the Blob with the specified Id.
        /// </summary>
        /// <param name="contactBlobId">The contacts id.</param>
        /// <returns></returns>
        public ContactListContainer GetAll(string contactBlobId)
        {
            BlobStorageManager mgr = new BlobStorageManager(_containerName);

            var stdContacts = new ContactListContainer
                                  {
                                      ContactList = mgr.GetEntitiesFromBlob<StdContact>(contactBlobId)
                                  };
            return stdContacts;
        }

        public bool WriteFullList(ContactListContainer elements, string contactBlobId, bool skipIfExisting)
        {
            try
            {
                BlobStorageManager mgr = new BlobStorageManager(_containerName);
                mgr.AddOrUpdateBlob(elements.ContactList, contactBlobId);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void DeleteBlob(string blobId)
        {
            BlobStorageManager mgr = new BlobStorageManager(_containerName);
            mgr.DeleteBlob(_containerName);
        }
    }
}
