namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    using SyncBase;

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactContainer
    {
        [DataMember]
        public StdContact Contact { get; set; }
    }
}