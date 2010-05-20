// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactService.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Service implementation for the <see cref="IContact" /> interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using Sem.Sync.Connector.Filesystem;
    using Sem.Sync.OnlineStorage.Properties;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Service implementation for the <see cref="IContact"/> interface.
    /// </summary>
    public class ContactService : IContact
    {
        #region Constants and Fields

        /// <summary>
        ///   The file system path to store the information.
        /// </summary>
        private readonly string storagePath = (new Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";

        #endregion

        #region Implemented Interfaces

        #region IContact

        /// <summary>
        /// Reads the contacts from a contact store specified in the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        /// <returns>
        /// A contact list container with the contacts from the folder. 
        /// </returns>
        public ContactListContainer GetAll(string clientFolderName)
        {
            var stdContacts = new ContactListContainer
                {
                   ContactList = new ContactClient().GetAll(this.storagePath).ToStdContacts() 
                };
            return stdContacts;
        }

        /// <summary>
        /// Writes contacts to a contact store specified in the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="elements">
        /// The elements to be written. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name.  
        /// </param>
        /// <param name="skipIfExisting">
        /// Ignored in this implementation. 
        /// </param>
        /// <returns>
        /// A value indicating whether the operation was successfull. 
        /// </returns>
        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            new ContactClient().WriteRange(elements.ContactList.ToStdElements(), this.storagePath);
            return true;
        }

        #endregion

        #endregion
    }
}