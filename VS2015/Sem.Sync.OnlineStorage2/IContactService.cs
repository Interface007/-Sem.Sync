namespace Sem.Sync.OnlineStorage2
{
    using System.IO;
    using System.ServiceModel;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContactService" in both code and config file together.
    /// <summary>
    /// Service interface to read and write contact data
    /// </summary>
    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IContactService
    {
        /// <summary>
        /// Reads contact information from the server
        /// </summary>
        /// <param name="clientFolderName"> The client folder name from where to read the contacts - this is a 
        /// logical folder, not a physical path on the server. </param>
        /// <returns> a container structure that holds a list of contacts </returns>
        [OperationContract]
        Stream GetAll(string clientFolderName);

        /// <summary>
        /// Writes contact information to the server
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <returns> A value indicating if the write operation was successfull. </returns>
        [OperationContract]
        bool WriteFullList(Stream elements);
    }
}
