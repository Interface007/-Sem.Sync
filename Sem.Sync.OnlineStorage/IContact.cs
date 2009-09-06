// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Service interface to read and write contact data
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.ServiceModel;

    /// <summary>
    /// Service interface to read and write contact data
    /// </summary>
    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IContact
    {
        /// <summary>
        /// Reads contact information from the server
        /// </summary>
        /// <param name="clientFolderName"> The client folder name from where to read the contacts - this is a 
        /// logical folder, not a physical path on the server. </param>
        /// <returns> a container structure that holds a list of contacts </returns>
        [OperationContract]
        ContactListContainer GetAll(string clientFolderName);

        /// <summary>
        /// Writes contact information to the server
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> The client folder name to write the contacts - this is a 
        /// logical folder, not a physical path on the server. </param>
        /// <param name="skipIfExisting"> Does not overwrite existing contacts if "True". </param>
        /// <returns> A value indicating if the write operation was successfull. </returns>
        [OperationContract]
        bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting);
    }
}
