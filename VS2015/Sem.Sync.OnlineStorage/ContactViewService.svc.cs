// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactViewService.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the service interface for getting view entities of the contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.Linq;

    using Sem.Sync.Connector.Filesystem;
    using Sem.Sync.OnlineStorage.Properties;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Implements the service interface for getting view entities of the contacts
    /// </summary>
    public class ContactViewService : IContactViewService
    {
        #region Constants and Fields

        /// <summary>
        ///   The path to store the contacts xml
        /// </summary>
        private readonly string storagePath = (new Settings()).StoragePath; // "C:\\ContactsServerData\\Contacts.xml";

        #endregion

        #region Implemented Interfaces

        #region IContactViewService

        /// <summary>
        /// Gets an array of vew entities from the contacts
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        /// <returns>
        /// the view entity array 
        /// </returns>
        public ViewContact[] GetAll(string clientFolderName)
        {
            var stdContacts = (from x in new ContactClient().GetAll(this.storagePath).ToStdContacts()
                               select
                                   new ViewContact
                                       {
                                           FullName = x.GetFullName(), 
                                           City =
                                               (x.PersonalAddressPrimary ??
                                                x.BusinessAddressPrimary ??
                                                new AddressDetail { CityName = string.Empty })
                                               .CityName, 
                                           Street =
                                               (x.PersonalAddressPrimary ??
                                                x.BusinessAddressPrimary ??
                                                new AddressDetail { StreetName = string.Empty }).StreetName, 
                                           Picture = x.PictureData
                                       }).ToArray();

            return stdContacts;
        }

        #endregion

        #endregion
    }
}