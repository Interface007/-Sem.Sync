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
                        ContactSchema.FileAs,
                    });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
        {
            // we default to accept certificates without trust because my testing area
            // does require ssl, but does not have a trusted certificate
            if (this.GetConfigValueBoolean("IgnoreCertificateErrors", true))
            {
                // Hack for debugging purposes to accept Fiddler certificate
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, errors) => true;
            }
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
                var contact = Contact.Bind(contactsFolder.Service, ((StdContact)element).ExternalIdentifier.GetProfileId(ProfileIdentifierType.ExchangeWs).Id);
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

                this.LogProcessingEvent(contact, Properties.Resources.CheckingForUpdateInsert);

                // In case of a EWS-id being present in the contact, we try to load the contact with suppressing
                // ServiceResponseException where ErrorCode == ErrorItemNotFound - otherwise we assume NULL
                var item = contact.ExternalIdentifier.GetProfileId(ProfileIdentifierType.ExchangeWs) != null
                           ? ExceptionHandler.Suppress(
                                () => Contact.Bind(
                                    contactsFolder.Service,
                                    contact.ExternalIdentifier.GetProfileId(ProfileIdentifierType.ExchangeWs).Id,
                                    ContactPropertySet),
                                (ServiceResponseException x) => x.ErrorCode == ServiceError.ErrorItemNotFound)
                           : null;

                // if we have not been able to load a contact, we save it as a new one
                if (item == null)
                {
                    this.LogProcessingEvent(contact, Properties.Resources.AddingContact);
                    var exchangeContact = contact.ToExchangeContact(service);
                    exchangeContact.Save(contactsFolderId);
                    contact.ExternalIdentifier.SetProfileId(ProfileIdentifierType.ExchangeWs, exchangeContact.Id.UniqueId);
                }
                else
                {
                    this.LogProcessingEvent(contact, Properties.Resources.UpdatingContact);
                    var exchangeContact = contact.ToExchangeContact(service);
                    exchangeContact.UpdateFromStdContact(contact);
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
                    this.LogProcessingEvent(contact, Properties.Resources.ElementLoaded);
                }

                CleanUpEntities(result);

                this.LogProcessingEvent(Properties.Resources.ElementsLoaded, result.Count);

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
                var strings = folderName.Split('|');
                server = strings[0];
                folderName = strings[1];
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