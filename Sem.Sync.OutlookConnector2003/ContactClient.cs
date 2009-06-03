// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling outlook contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OutlookConnector2003
{
    #region usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;

    using Microsoft.Office.Interop.Outlook;

    using SyncBase;
    using Properties;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling outlook contacts
    /// </summary>
    public class ContactClient : StdClient
    {
        #region interface IClientBase
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var currentElementName = "";

            // get a connection to outlook 
            LogProcessingEvent(Resources.uiLogginIn);
            var outlookNamespace = OutlookClient.GetNameSpace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                // select a folder
                var outlookFolder = OutlookClient.GetOutlookMAPIFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts);

                // if no folder has been selected, we will leave here
                if (outlookFolder == null)
                {
                    LogProcessingEvent(Resources.uiNoFolderSelected);
                }
                else
                {
                    // get all the Contacts from the Contacts Folder 
                    var contactItems = outlookFolder.Items;

                    // iterate through the contacts
                    for (var itemIndex = 1; itemIndex <= contactItems.Count; itemIndex++)
                    {
                        // in case of problems with a single item, we will continue with the next
                        try
                        {
                            var contactItem = contactItems[itemIndex] as ContactItem;
                            if (contactItem != null)
                            {
                                currentElementName = contactItem.LastName + contactItem.FirstName;

                                LogProcessingEvent(Resources.uiReadingContact, currentElementName);

                                result.Add(OutlookClient.ConvertToStandardContact(contactItem));
                            }
                        }
                        catch (System.Runtime.InteropServices.COMException ex)
                        {
                            if (ex.ErrorCode == -1285291755 ||
                                ex.ErrorCode == -2147221227)
                            {
                                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiProblemAccessingOutlookStore, currentElementName, ex.Message));
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (System.Exception pObjException)
            {
                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiErrorAtName, currentElementName, pObjException.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            return result;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiAddingXElements, elements.Count));

            // create outlook instance and get the folder
            var outlookNamespace = OutlookClient.GetNameSpace();
            var contactsEnum = OutlookClient.GetOutlookMAPIFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderContacts).Items;

            // extract the contacts that do already exist
            var contactsList = OutlookClient.GetContactsList(contactsEnum);

            var added = 0;
            foreach (var element in elements)
            {
                // find outlook contact with matching id, create new if needed
                LogProcessingEvent(element, Resources.uiSearching);
                if (OutlookClient.WriteContactToOutlook(contactsEnum, (StdContact)element, skipIfExisting, contactsList))
                {
                    LogProcessingEvent(element, Resources.uiContactUpdated);
                    added++;
                }
            }

            outlookNamespace.Logoff();
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiXElementsAdded, added));
        }
        
        /// <summary>
        /// detects duplicates and removes them from the contacts
        /// </summary>
        /// <param name="pathToStore"></param>
        public override void RemoveDuplicates(string pathToStore)
        {
            var currentElementName = "";

            // get a connection to outlook 
            LogProcessingEvent(Resources.uiLogginIn);
            var outlookNamespace = OutlookClient.GetNameSpace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                var calendarItems = OutlookClient.GetOutlookMAPIFolder(outlookNamespace, pathToStore, OlDefaultFolders.olFolderContacts);

                LogProcessingEvent(Resources.uiPreparingList);
                var outlookItemList = from a in calendarItems.Items.OfType<ContactItem>()
                                       orderby a.LastName, a.FirstName
                                       select a;

                _ContactItem lastItem = null;
                foreach (var item in outlookItemList)
                {
                    currentElementName = item.LastName + ", " + item.FirstName;

                    if (lastItem != null)
                    {
                        var stdItem = OutlookClient.ConvertToStandardContact(item);
                        LogProcessingEvent(stdItem, Resources.uiComparing);

                        if (lastItem.LastName == item.LastName
                            && lastItem.FirstName == item.FirstName
                            && lastItem.MiddleName == item.MiddleName)
                        {

                            LogProcessingEvent(stdItem, Resources.uiRemoving);

                            item.Delete();
                            continue;
                        }
                    }
                    lastItem = item;
                }
            }
            catch (System.Exception ex)
            {
                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiErrorAtName, currentElementName, ex.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            LogProcessingEvent(Resources.uiRemoveDuplicates);
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="element">the element to add</param>
        /// <param name="clientFolderName">the outlook folder to use</param>
        public override void AddItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="elements">list of all the elements to add</param>
        /// <param name="clientFolderName">the outlook folder to use</param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            WriteFullList(elements, clientFolderName, false);
        }

        public override void MergeMissingItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            WriteFullList(elements, clientFolderName, true);
        }

        public override void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// Gets the ui friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "Outlook-Contact-Connector"; }
        }

        #endregion
    }
}