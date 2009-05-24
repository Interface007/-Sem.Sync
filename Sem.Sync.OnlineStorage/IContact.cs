namespace Sem.Sync.OnlineStorage
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    using SyncBase;

    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IContact
    {

        [OperationContract]
        ContactListContainer GetAll(string clientFolderName);

        [OperationContract]
        bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting);

    }

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactContainer
    {
        [DataMember]
        public StdContact Contact { get; set; }
    }

    [DataContract]
    public class ContactListContainer
    {
        [DataMember]
        public List<StdContact> ContactList { get; set; }
    }
}
