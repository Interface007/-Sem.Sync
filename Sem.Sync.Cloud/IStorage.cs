// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Service interface for interacting with the contact storage service
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    using System.ServiceModel;

    /// <summary>
    /// Service interface for interacting with the contact storage service
    /// </summary>
    [ServiceContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public interface IStorage
    {
        #region Public Methods

        /// <summary>
        /// Deletes all contacts from a specified folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id to delete the content from. 
        /// </param>
        /// <returns>
        /// A value indicating the success of the operation.  
        /// </returns>
        [OperationContract]
        BooleanResultContainer DeleteBlob(string folderId);

        /// <summary>
        /// Reads the content of a "folder".
        /// </summary>
        /// <param name="folderId">
        /// The folder id. 
        /// </param>
        /// <returns>
        /// All contacts from that folder. 
        /// </returns>
        [OperationContract]
        ContactListContainer GetAll(string folderId);

        /// <summary>
        /// Writes contacts to a "folder".
        /// </summary>
        /// <param name="elements">
        /// The elements to be written. 
        /// </param>
        /// <param name="folderId">
        /// The folder id.  
        /// </param>
        /// <param name="skipIfExisting">
        /// Skips existing contacts if implemented by the concrete service. 
        /// </param>
        /// <returns>
        /// A value indicating the success of the operation.  
        /// </returns>
        [OperationContract]
        BooleanResultContainer WriteFullList(ContactListContainer elements, string folderId, bool skipIfExisting);

        #endregion
    }
}