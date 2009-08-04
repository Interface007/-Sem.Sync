﻿namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    
    using SyncBase;

    // NOTE: If you change the interface name "IStorage" here, you must also update the reference to "IStorage" in Web.config.
    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IStorage
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
