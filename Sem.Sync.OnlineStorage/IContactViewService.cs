// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContactViewService.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IContactViewService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.ServiceModel;

    /// <summary>
    /// Interface to get a subset of information about contacts
    /// </summary>
    [ServiceContract]
    public interface IContactViewService
    {
        /// <summary>
        /// Gets a subset of contact information needed to show a contact in a GUI
        /// </summary>
        /// <param name="clientFolderName"> The client folder name. </param>
        /// <returns> an array of contact information </returns>
        [OperationContract]
        ViewContact[] GetAll(string clientFolderName);
    }
}
