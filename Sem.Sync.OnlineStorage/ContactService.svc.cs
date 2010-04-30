// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactService.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using Connector.Filesystem;
    using SyncBase.Helpers;

    /// <summary>
    /// Service implementation for the <see cref="IContact"/> interface.
    /// </summary>
    public class ContactService : IContact
    {
        /// <summary>
        /// The file system path to store the information.
        /// </summary>
        private readonly string storagePath = (new Properties.Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";

        /// <summary>
        /// Reads the contacts from a contact store specified in the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="clientFolderName"> The client folder name. </param>
        /// <returns> A contact list container with the contacts from the folder. </returns>
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
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> The client folder name.  </param>
        /// <param name="skipIfExisting"> Ignored in this implementation. </param>
        /// <returns> A value indicating whether the operation was successfull. </returns>
        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            new ContactClient().WriteRange(elements.ContactList.ToStdElements(), this.storagePath);
            return true;
        }
    }
}
