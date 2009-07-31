namespace Sem.Sync.Cloud
{
    using GenericHelpers;

    using SyncBase;
    using SyncBase.Helpers;

    // NOTE: If you change the class name "Storage" here, you must also update the reference to "Storage" in Web.config.
    public class Storage : IStorage
    {
        /// <summary>
        /// The default file system path to store the information.
        /// </summary>
        private const string storagePath = "DefaultStorage";
        private static readonly Factory factory = new Factory("Sem.Sync.Cloud");
        private readonly StdClient connector = factory.GetNewObject<StdClient>("{Connector}");

        public ContactListContainer GetAll(string clientFolderName)
        {
            var stdContacts = new ContactListContainer
            {
                ContactList = connector.GetAll(storagePath).ToContacts()
            };
            return stdContacts;
        }

        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            connector.WriteRange(elements.ContactList.ToStdElement(), storagePath);
            return true;
        }
    }
}
