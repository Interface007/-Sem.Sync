// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling outlook calendar items
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Microsoft.Office.Interop.Outlook;

    using Sem.Sync.Connector.Outlook.Properties;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    using Exception = System.Exception;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling outlook calendar items
    /// </summary>
    [ConnectorDescription(DisplayName = "Microsoft Outlook 2007", CanReadContacts = false, CanWriteContacts = false, 
        CanReadCalendarEntries = true, CanWriteCalendarEntries = true)]
    public class CalendarClient : StdClient
    {
        #region Properties

        /// <summary>
        ///   Gets the ui friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Outlook-Calendar-Connector";
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
        /// Implementation of the process of writing a single element and skipping this process if this 
        ///   element is already present. If the element does not exist, it will be added. If it does exist
        ///   the element will not be added and not be overridden.
        /// </summary>
        /// <param name="element">
        /// the element to be added
        /// </param>
        /// <param name="clientFolderName">
        /// the information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public override void MergeMissingItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        ///   skipping this process if an element is already present. Missing elements will be added, existing 
        ///   elements will not be altered.
        /// </summary>
        /// <param name="elements">
        /// the elements to be added in a list of elements
        /// </param>
        /// <param name="clientFolderName">
        /// the information where inside the source the elements reside - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        public override void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// detects duplicates and removes them from the calendar
        /// </summary>
        /// <param name="clientFolderName">
        /// the path to the outlook folder to process
        /// </param>
        public override void RemoveDuplicates(string clientFolderName)
        {
            var currentElementName = string.Empty;

            // get a connection to outlook 
            this.LogProcessingEvent(Resources.UserInfoLoggingOn);
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                var calendarItems = OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar);

                this.LogProcessingEvent(Resources.UserInfoPreparingList);
                var outlookItemList = from a in calendarItems.Items.OfType<AppointmentItem>()
                                      orderby a.Subject , a.Start
                                      select a;

                AppointmentItem lastItem = null;
                var lastPersonName = string.Empty;
                foreach (
                    var item in
                        outlookItemList.OrderBy(x => x.Start.ToString("MM.dd-hh:mm", CultureInfo.InvariantCulture)))
                {
                    var subject = item.Subject;

                    var personName = string.IsNullOrEmpty(subject)
                                         ? string.Empty
                                         : subject.StartsWith("Geburtstag von ", StringComparison.OrdinalIgnoreCase)
                                               ? subject.Substring(15)
                                               : subject.EndsWith("'s Birthday", StringComparison.OrdinalIgnoreCase)
                                                     ? subject.Substring(0, subject.Length - 11)
                                                     : string.Empty;

                    if (lastItem != null)
                    {
                        var stdItem = OutlookClient.ConvertToStandardCalendarItem(item, null);
                        this.LogProcessingEvent(stdItem, "comparing ...");

                        if (!string.IsNullOrEmpty(personName))
                        {
                            if (lastPersonName == personName && lastItem.Start == item.Start &&
                                lastItem.Body == item.Body)
                            {
                                this.LogProcessingEvent(stdItem, Resources.UserInfoRemoving);

                                item.Delete();
                                continue;
                            }
                        }
                    }

                    lastItem = item;
                    lastPersonName = personName;
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(
                    string.Format(
                        CultureInfo.CurrentCulture, Resources.UserInfoErrorAtName, currentElementName, ex.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            this.LogProcessingEvent(Resources.UserInfoRemoveDuplicatesFinished);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var currentElementName = string.Empty;
            var minimumDate = DateTime.Now;

            // get a connection to outlook 
            this.LogProcessingEvent(Resources.UserInfoLoggingOn);
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                if (!string.IsNullOrEmpty(clientFolderName) && clientFolderName.Contains(":"))
                {
                    clientFolderName = clientFolderName.Substring(
                        0, clientFolderName.IndexOf(":", StringComparison.Ordinal));
                }

                // select a folder
                var outlookFolder = OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar);

                // if no folder has been selected, we will leave here
                if (outlookFolder == null)
                {
                    this.LogProcessingEvent(Resources.UserInfoNoOutlookFolderSelected);
                }
                else
                {
                    // get all the calendar items from the calendar Folder 
                    var calendarItems = outlookFolder.Items;

                    // iterate through the calendarFolder
                    for (var itemIndex = 1; itemIndex <= calendarItems.Count; itemIndex++)
                    {
                        // in case of problems with a single item, we will continue with the next
                        try
                        {
                            var calendarStdItem = calendarItems[itemIndex] as AppointmentItem;
                            if (calendarStdItem != null && calendarStdItem.Start > minimumDate)
                            {
                                currentElementName =
                                    calendarStdItem.Start.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CurrentCulture) +
                                    " - " + calendarStdItem.Subject;

                                this.LogProcessingEvent(Resources.UserInfoReading + currentElementName);

                                result.Add(
                                    OutlookClient.ConvertToStandardCalendarItem(
                                        calendarStdItem, result.ToOtherType<StdElement, StdCalendarItem>()));
                            }
                        }
                        catch (COMException ex)
                        {
                            if (ex.ErrorCode == -1285291755 || ex.ErrorCode == -2147221227)
                            {
                                this.LogProcessingEvent(
                                    string.Format(
                                        CultureInfo.CurrentCulture, 
                                        Resources.UserInfoProblemAccessingStore, 
                                        currentElementName, 
                                        ex.Message));
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(
                    string.Format(
                        CultureInfo.CurrentCulture, Resources.UserInfoErrorAtName, currentElementName, ex.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            return result;
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// the information to where inside the source the elements should be written - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting">
        /// specifies whether existing elements should be updated or simply left as they are
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.LogProcessingEvent(
                string.Format(CultureInfo.CurrentCulture, Resources.UserInfoAddingEntries, elements.Count));

            // create outlook instance and get the folder
            var outlookNamespace = OutlookClient.GetNamespace();
            var appointmentEnum =
                OutlookClient.GetOutlookMapiFolder(
                    outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar).Items;

            // extract the contacts that do already exist
            var appointmentList = OutlookClient.GetAppointmentsList(appointmentEnum);

            var added = 0;
            var updated = 0;

            foreach (var element in elements)
            {
                // find outlook contact with matching id, create new if needed
                this.LogProcessingEvent(element, Resources.UserInfoSearchingElementInStore);
                switch (
                    OutlookClient.WriteCalendarItemToOutlook(appointmentEnum, (StdCalendarItem)element, appointmentList)
                    )
                {
                    case SaveAction.None:
                        break;
                    case SaveAction.Update:
                        updated++;
                        break;
                    case SaveAction.Create:
                        added++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            outlookNamespace.Logoff();
            this.LogProcessingEvent(
                string.Format(CultureInfo.CurrentCulture, Resources.UserInfoElementsAdded, added, updated));
        }

        #endregion
    }
}