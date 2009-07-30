namespace Sem.Sync.Cloud
{
    using CloudStorageConnector;
    using SyncBase.Helpers;

    // NOTE: If you change the class name "Storage" here, you must also update the reference to "Storage" in Web.config.
    public class Storage : IStorage
    {
        /// <summary>
        /// The file system path to store the information.
        /// </summary>
        private readonly string storagePath = "DefaultStorage"; // "C:\\ContactsServerData\\Contacts.xml";

        public ContactListContainer GetAll(string clientFolderName)
        {
            var stdContacts = new ContactListContainer
            {
                ContactList = new ContactClient().GetAll(this.storagePath).ToContacts()
            };
            return stdContacts;
        }

        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            new ContactClient().WriteRange(elements.ContactList.ToStdElement(), this.storagePath);
            return true;
        }
    }
}
