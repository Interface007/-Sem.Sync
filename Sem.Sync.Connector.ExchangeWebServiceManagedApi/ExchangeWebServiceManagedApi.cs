// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExchangeWebServiceManagedApi.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements a connector to Microsoft Exchange Web Services via Managed API
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.ExchangeWebServiceManagedApi
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    
    using Microsoft.Exchange.WebServices.Data;
    
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Implements a connector to Microsoft Exchange Web Services via Managed API
    /// </summary>
    [ClientStoragePathDescription(
        Mandatory = true,
        Default = "",
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Microsoft Exchange Web Services via Managed API",
        CanWriteContacts = true,
        CanReadContacts = true,
        MatchingIdentifier = ProfileIdentifierType.Default,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// The set of properties to be processed
        /// </summary>
        private static readonly PropertySet ContactPropertySet;

        /// <summary>
        /// Initializes static members of the <see cref="ContactClient"/> class.
        /// </summary>
        static ContactClient()
        {
            ContactPropertySet = new PropertySet(
                BasePropertySet.FirstClassProperties,
                new PropertyDefinitionBase[]
                    {
                        ItemSchema.Categories,
                        ItemSchema.Body,
                    });
        }

        /// <summary>
        /// Gets a friendly name that can be shown to the user.
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "ExchangeWSMA-Connector"; }
        }

        /// <summary>
        /// Deletes the items specified.
        /// </summary>
        /// <param name="elementsToDelete"> The elements to delete. </param>
        /// <param name="clientFolderName"> The client folder name of the items to delete. </param>
        public override void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
            var contactsFolder = this.GetContactsFolder(clientFolderName);
            foreach (var element in elementsToDelete)
            {
                var contact = Contact.Bind(contactsFolder.Service, ((StdContact)element).PersonalProfileIdentifiers.ExchangeWs.Id);
                contact.Delete(DeleteMode.MoveToDeletedItems);
            }
        }

        /// <summary>
        /// Implements the write operation of this connector
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <param name="clientFolderName"> The exchange contact folder name. </param>
        /// <param name="skipIfExisting"> Ignored in this connector. </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var contactsFolder = this.GetContactsFolder(clientFolderName);
            var contactsFolderId = contactsFolder.Id;
            var service = contactsFolder.Service;

            foreach (var element in elements)
            {
                var contact = (StdContact)element;

                // In case of a EWS-id being present in the contact, we try to load the contact with suppressing
                // ServiceResponseException where ErrorCode == ErrorItemNotFound - otherwise we assume NULL
                var item = contact.PersonalProfileIdentifiers.ExchangeWs != null
                           ? ExceptionHandler.Suppress(
                                () => Contact.Bind(
                                    contactsFolder.Service,
                                    contact.PersonalProfileIdentifiers.ExchangeWs.Id,
                                    ContactPropertySet),
                                (ServiceResponseException x) => x.ErrorCode == ServiceError.ErrorItemNotFound)
                           : null;

                // if we have not been able to load a contact, we save it as a new one
                if (item == null)
                {
                    this.LogProcessingEvent(contact, "adding contact");
                    var exchangeContact = contact.ToExchangeContact(service);
                    exchangeContact.Save(contactsFolderId);
                    contact.PersonalProfileIdentifiers.ExchangeWs = exchangeContact.Id.UniqueId;
                }
                else
                {
                    // todo: currenty we don't touch existing entries, but we should update them
                    this.LogProcessingEvent(contact, "not written, because already existing");
                }
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName"> The exchange folder name to read from. </param>
        /// <param name="result"> The result list. </param>
        /// <returns> The list of <see cref="StdContact"/> elements from the source. </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var contactsFolder = this.GetContactsFolder(clientFolderName);

            var offset = 0;
            var itemsLeft = true;
            while (itemsLeft)
            {
                var resultList = contactsFolder.FindItems(
                    new ItemView(
                        100,
                        offset,
                        OffsetBasePoint.Beginning)
                        {
                            PropertySet = BasePropertySet.IdOnly,
                            Traversal = ItemTraversal.Shallow
                        });

                itemsLeft = resultList.Items.Count == 100;

                foreach (var element in resultList)
                {
                    var currentItem = element as Contact;
                    if (currentItem == null)
                    {
                        continue;
                    }

                    StdContact contact = null;
                    ExceptionHandler.Suppress(
                        () =>
                            {
                                var exchangeContact = Contact.Bind(contactsFolder.Service, currentItem.Id);
                                contact = exchangeContact.ToStdContact();
                            },
                        (Exception ex) => true);

                    if (contact == null || string.IsNullOrEmpty(contact.Name.ToString()))
                    {
                        continue;
                    }

                    result.Add(contact);
                    this.LogProcessingEvent(contact, "element loaded ...");
                }

                CleanUpEntities(result);

                this.LogProcessingEvent("{0} elements loaded ...", result.Count);

                offset += 100;
            }

            return result;
        }

        /// <summary>
        /// Opens a contact folder using the managed api
        /// </summary>
        /// <param name="folderName"> The folder name to open. </param>
        /// <returns> the contacts folder with the specified name </returns>
        /// <exception cref="TechnicalException"> "Unable to determine Exchange server URL." in case of configuration or AutoDiscovery issues </exception>
        private Folder GetContactsFolder(string folderName)
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1)
                              {
                                  Credentials = new NetworkCredential(
                                      this.LogOnUserId,
                                      this.LogOnPassword,
                                      this.LogOnDomain),
                              };

            var server = this.GetConfigValue("ServerUrl");

            if (folderName.Contains("|"))
            {
                server = folderName.Split('|')[0];
                folderName = folderName.Split('|')[1];
            }

            if (string.IsNullOrEmpty(server))
            {
                throw new TechnicalException(
                    "Unable to determine Exchange server URL.",
                    null,
                    new KeyValuePair<string, object>("configured server url", this.GetConfigValue("ServerUrl")),
                    new KeyValuePair<string, object>("folderName", folderName));
            }

            if (server.Contains("@"))
            {
                service.AutodiscoverUrl(server);
            }
            else
            {
                service.Url = new Uri(server);
            }

            if (string.IsNullOrEmpty(folderName))
            {
                return Folder.Bind(service, WellKnownFolderName.Contacts);
            }

            var findResults = service.FindFolders(
                WellKnownFolderName.Contacts,
                new SearchFilter.SearchFilterCollection(
                    LogicalOperator.And,
                    new SearchFilter[]
                        {
                            new SearchFilter.IsEqualTo(FolderSchema.DisplayName, folderName)
                        }),
                new FolderView(10)
                    {
                        PropertySet = new PropertySet(
                            BasePropertySet.IdOnly,
                            new PropertyDefinitionBase[]
                                {
                                    FolderSchema.DisplayName,
                                    FolderSchema.Id
                                }),
                        Traversal = FolderTraversal.Deep,
                    });

            if (findResults.TotalCount == 0)
            {
                throw new TechnicalException(
                    "Unable to find Exchange folder.",
                    null,
                    new KeyValuePair<string, object>("configured server url", this.GetConfigValue("ServerUrl")),
                    new KeyValuePair<string, object>("folderName", folderName));
            }

            return findResults.Folders[0];
        }
    }
}