namespace Sem.Sync.OnlineStorage
{
    using SyncBase.Helpers;
    using FilesystemConnector;
    using System.ServiceModel;

    public class ContactService : IContact
    {
        private string StoragePath = (new Properties.Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";
        public ContactListContainer GetAll(string clientFolderName)
        {
            var stdContacts = new ContactListContainer
                                  {
                                      ContactList = new ContactClient().GetAll(StoragePath).ToContacts()
                                  };
            return stdContacts;
        }

        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            new ContactClient().WriteRange(elements.ContactList.ToStdElement(), StoragePath);
            return true;
        }
    }
}
