namespace Sem.Sync.OnlineStorage
{
    using System;

    using System.Linq;
    using SyncBase.Helpers;
    using FilesystemConnector;
    using System.ServiceModel;
    using Sem.Sync.SyncBase;

    public class ContactViewService : IContactViewService
    {
        private string StoragePath = (new Properties.Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";
        public ViewContact[] GetAll(string clientFolderName)
        {
            ViewContact[] stdContacts;

            stdContacts = 
                (from x in new ContactClient().GetAll(StoragePath).ToContacts()
                 select new ViewContact{ FullName = x.GetFullName()}
                ).ToArray();
            
            return stdContacts;
        }
    }
}
