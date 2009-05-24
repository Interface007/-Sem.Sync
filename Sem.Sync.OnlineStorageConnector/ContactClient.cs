namespace Sem.Sync.OnlineStorageConnector
{
    using System.Collections.Generic;

    using SyncBase;
    using SyncBase.Helpers;

    using OnlineStorage;

    public class ContactClient : StdClient
    {
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var client = new OnlineStorage.ContactClient();
            var contacts = client.GetAll(clientFolderName).ContactList;
            foreach (var contact in contacts)
            {
                result.Add(contact);
            }
            return result;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var client = new OnlineStorage.ContactClient();
            client.WriteFullList(new ContactListContainer
                                     {
                                         ContactList = elements.ToContacts().ToArray()
                                     }, 
                                     clientFolderName, 
                                     skipIfExisting);
        }

        public override string FriendlyClientName
        {
            get 
            {
                return "OnlineStorage-Connector";
            }
        }
    }
}
