namespace Sem.Sync.Cloud
{
    using System.ServiceModel;

    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IStorage
    {
        [OperationContract]
        ContactListContainer GetAll(string blobId);

        [OperationContract]
        BooleanResultContainer WriteFullList(ContactListContainer elements, string blobId, bool skipIfExisting);

        [OperationContract]
        BooleanResultContainer DeleteBlob(string blobId);

    }
}
