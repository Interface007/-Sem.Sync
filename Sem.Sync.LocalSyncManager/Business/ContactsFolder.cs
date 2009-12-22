// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactsFolder.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Represents a contacts folder for binding UI elements
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.Business
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using SyncBase;
    using SyncBase.Interfaces;

    /// <summary>
    /// Represents a contacts folder for binding UI elements
    /// </summary>
    public class ContactsFolder : INotifyPropertyChanged
    {
        private IEnumerable<StdContact> contacts;

        private StdContact currentContact;

        public ContactsFolder(IClientBase connector, string clientPath)
        {
            this.Contacts = (IEnumerable<StdContact>)connector.GetAll(clientPath);
        }

        /// <summary>
        /// Gets or sets the list of Contacts.
        /// </summary>
        public IEnumerable<StdContact> Contacts
        {
            get
            {
                return this.contacts;
            }

            set
            {
                this.contacts = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        public StdContact CurrentContact
        {
            get
            {
                return this.currentContact;
            }

            set
            {
                this.currentContact = value;
                this.RaisePropertyChanged("CurrentContact");
            }
        }

        /// <summary>
        /// Calls the event to inform other classes about an internal change of this objects 
        /// state - this will cause the GUI to read the data from this object.
        /// </summary>
        /// <param name="propertyName"> The property name that has been changed. </param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
