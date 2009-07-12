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
    using FilesystemConnector;
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

        public ContactListContainer GetAll(string clientFolderName)
        {
            var stdContacts = new ContactListContainer
                                  {
                                      ContactList = new ContactClient().GetAll(this.storagePath).ToContacts()
                                  };
            return stdContacts;
        }

        public bool WriteFullList(ContactListContainer elements, string clientFolderName, bool skipIfExisting)
        {
            new ContactClient().WriteRange(elements.ContactList.ToStdElement(), this.storagePath);
            return true;
        }
    }
}
