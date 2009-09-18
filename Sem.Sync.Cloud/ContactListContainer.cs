namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    using SyncBase;

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactListContainer
    {
        [DataMember]
        public StdContact[] ContactList { get; set; }

        [DataMember]
        public TechnicalMessage[] messages { get; set; }
    }
}