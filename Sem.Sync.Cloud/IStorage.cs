namespace Sem.Sync.Cloud
{
    using System.ServiceModel;

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
}
