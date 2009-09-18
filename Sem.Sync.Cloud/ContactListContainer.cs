namespace Sem.Sync.Cloud
{
    using SyncBase;

    public class ContactListContainer
    {
        public StdContact[] ContactList { get; set; }
        public TechnicalMessage[] messages { get; set; }
    }
}