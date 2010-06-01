// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling outlook contacts. The outlook connector does use the
//   ProfileIdentifierType.Default, which is idential to the <see cref="StdContact.Id" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook2003
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Microsoft.Office.Interop.Outlook;

    using Sem.Sync.Connector.Outlook2003.Properties;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    using Exception = System.Exception;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling outlook contacts. The outlook connector does use the
    ///   ProfileIdentifierType.Default, which is idential to the <see cref="StdContact.Id"/>.
    /// </summary>
    [ConnectorDescription(DisplayName = "Microsoft Outlook 2003", CanReadContacts = true, CanWriteContacts = true,
        Internal = false, IsGeneric = false, MatchingIdentifier = ProfileIdentifierType.Default)]
    [ClientStoragePathDescriptionAttribute(Default = "", ReferenceType = ClientPathType.Undefined)]
    public class ContactClient : StdClient
    {
        #region Properties

        /// <summary>
        ///   Gets the ui friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Outlook-Contact-Connector";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="element">
        /// the element to add
        /// </param>
        /// <param name="clientFolderName">
        /// the outlook folder to use
        /// </param>
        public override void AddItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="elements">
        /// list of all the elements to add
        /// </param>
        /// <param name="clientFolderName">
        /// the outlook folder to use
        /// </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Deletes a contact entry.
        /// </summary>
        /// <param name="elementsToDelete">
        /// The elements to delete. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        public override void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
            if (elementsToDelete == null)
            {
                return;
            }
            
            var outlookNamespace = OutlookClient.GetNamespace();
            var outlookFolder = OutlookClient.GetOutlookMapiFolder(
                outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts);

            elementsToDelete.ForEach(
                x =>
                {
                    // todo: the filter needs to be corrected!
                    var contact = outlookFolder.Items.Find(x.Id.ToString()) as ContactItem;
                    if (contact != null)
                    {
                        contact.Delete();
                    }
                });

            outlookNamespace.Logoff();
        }

        /// <summary>
        /// overrides the standard MergeMissingItem method to not read all outlook contacts, because
        ///   that is not needed to add a new contact
        /// </summary>
        /// <param name="element">
        /// element that should be merged into the outlook folder
        /// </param>
        /// <param name="clientFolderName">
        /// outlook folder that should get the new contact
        /// </param>
        public override void MergeMissingItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// overrides the standard MergeMissingItem method to not read all outlook contacts, because
        ///   that is not needed to add a new contact
        /// </summary>
        /// <param name="elements">
        /// list of elements that should be merged into the outlook folder
        /// </param>
        /// <param name="clientFolderName">
        /// outlook folder that should get the new contacts
        /// </param>
        public override void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// detects duplicates and removes them from the contacts
        /// </summary>
        /// <param name="clientFolderName">
        /// the outlook path that should be searched for duplicates
        /// </param>
        public override void RemoveDuplicates(string clientFolderName)
        {
            var currentElementName = string.Empty;

            // get a connection to outlook 
            this.LogProcessingEvent(Resources.uiLogginIn);
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                var calendarItems = OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts);

                this.LogProcessingEvent(Resources.uiPreparingList);
                var outlookItemList = from a in calendarItems.Items.OfType<ContactItem>()
                                      orderby a.LastName, a.FirstName
                                      select a;

                ContactItem lastItem = null;
                var contactList = new List<StdContact>(300);
                foreach (var item in outlookItemList)
                {
                    currentElementName = item.LastName + ", " + item.FirstName;

                    if (lastItem != null)
                    {
                        var stdItem = OutlookClient.ConvertToStandardContact(item, contactList);
                        contactList.Add(stdItem);
                        this.LogProcessingEvent(stdItem, Resources.uiComparing);

                        if (lastItem.LastName == item.LastName && lastItem.FirstName == item.FirstName &&
                            lastItem.MiddleName == item.MiddleName)
                        {
                            this.LogProcessingEvent(stdItem, Resources.uiRemoving);

                            item.Delete();
                            continue;
                        }
                    }

                    lastItem = item;
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(Resources.uiErrorAtName, currentElementName, ex.Message);
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            this.LogProcessingEvent(Resources.uiRemoveDuplicates);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">
        /// the name of the outlook folder that does contain the contacts that will be read.
        /// </param>
        /// <param name="result">
        /// A list of StdElements that will get the new imported entries.
        /// </param>
        /// <returns>
        /// The list with the added contacts
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            var currentElementName = string.Empty;

            // get a connection to outlook 
            this.LogProcessingEvent(Resources.uiLogginIn);
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                // select a folder
                var outlookFolder = OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts);

                // if no folder has been selected, we will leave here
                if (outlookFolder == null)
                {
                    this.LogProcessingEvent(Resources.uiNoFolderSelected);
                }
                else
                {
                    // get all the Contacts from the Contacts Folder 
                    var contactItems = outlookFolder.Items;
                    var itemsToDo = contactItems.Count;

                    // iterate through the contacts
                    for (var itemIndex = 1; itemIndex <= itemsToDo; itemIndex++)
                    {
                        // in case of problems with a single item, we will continue with the next
                        try
                        {
                            var contactItem = contactItems[itemIndex] as ContactItem;
                            if (contactItem != null)
                            {
                                currentElementName = contactItem.LastName + ", " + contactItem.FirstName;

                                var newContact = OutlookClient.ConvertToStandardContact(
                                    contactItem, result.ToStdContacts());
                                if (newContact != null)
                                {
                                    result.Add(newContact);
                                    this.LogProcessingEvent(
                                        newContact,
                                        string.Format(
                                            CultureInfo.CurrentCulture, Resources.uiReadingContact, currentElementName));
                                }
                            }
                        }
                        catch (COMException ex)
                        {
                            if (ex.ErrorCode == -1285291755 || ex.ErrorCode == -2147221227)
                            {
                                this.LogProcessingEvent(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Resources.uiProblemAccessingOutlookStore,
                                        currentElementName,
                                        ex.Message));
                            }
                            else
                            {
                                throw;
                            }
                        }

                        this.UpdateProgress(itemIndex * 100 / itemsToDo);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(
                    string.Format(CultureInfo.CurrentCulture, Resources.uiErrorAtName, currentElementName, ex.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements">
        /// The elements to be written to outlook.
        /// </param>
        /// <param name="clientFolderName">
        /// the name of the outlook folder that will get the contacts while writing data.
        /// </param>
        /// <param name="skipIfExisting">
        /// a value indicating whether existing entries should be added overwritten or skipped.
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.LogProcessingEvent(
                string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));

            // create outlook instance and get the folder
            var outlookNamespace = OutlookClient.GetNamespace();
            var contactsEnum =
                OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts).Items;

            // extract the contacts that do already exist
            var contactsList = OutlookClient.GetContactsList(contactsEnum);

            var added = 0;
            foreach (var element in elements)
            {
                // find outlook contact with matching id, create new if needed
                this.LogProcessingEvent(element, Resources.uiSearching);
                if (
                    !OutlookClient.WriteContactToOutlook(
                        contactsEnum, (StdContact)element, skipIfExisting, contactsList))
                {
                    continue;
                }

                this.LogProcessingEvent(element, Resources.uiContactUpdated);
                added++;
            }

            outlookNamespace.Logoff();
            this.LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, added));
        }

        #endregion
    }
}