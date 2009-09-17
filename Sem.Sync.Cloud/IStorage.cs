namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    using SyncBase;

    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IStorage
    {
        [OperationContract]
        ContactListContainer GetAll(string blobId);

        [OperationContract]
        bool WriteFullList(ContactListContainer elements, string blobId, bool skipIfExisting);

        [OperationContract]
        void DeleteBlob(string blobId);

    }

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactContainer
    {
        [DataMember]
        public StdContact Contact { get; set; }
    }

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class TechnicalMessage
    {
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactListContainer
    {
        [DataMember]
        public List<StdContact> ContactList { get; set; }

        [DataMember]
        public List<TechnicalMessage> messages { get; set; }
    }
}
