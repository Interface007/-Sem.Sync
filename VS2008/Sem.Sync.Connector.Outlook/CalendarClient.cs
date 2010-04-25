// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
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
    using Microsoft.Office.Interop.Outlook;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling outlook calendar items
    /// </summary>
    [ConnectorDescription(DisplayName = "Outlook Calendar Connector 2007",
        CanReadContacts = false,
        CanWriteContacts = false,
        CanReadCalendarEntries = true,
        CanWriteCalendarEntries = true)]
    public class CalendarClient : StdClient
    {
        /// <summary>
        /// Gets the ui friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "Outlook-Calendar-Connector"; }
        }

        /// <summary>
        /// detects duplicates and removes them from the calendar
        /// </summary>
        /// <param name="clientFolderName">the path to the outlook folder to process</param>
        public override void RemoveDuplicates(string clientFolderName)
        {
            var currentElementName = string.Empty;

            // get a connection to outlook 
            LogProcessingEvent("logging on ...");
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                var calendarItems = OutlookClient.GetOutlookMapiFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar);

                LogProcessingEvent("preparing list ...");
                var outlookItemList = from a in calendarItems.Items.OfType<AppointmentItem>()
                                      orderby a.Subject, a.Start
                                      select a;

                AppointmentItem lastItem = null;
                foreach (var item in outlookItemList)
                {
                    currentElementName = item.Subject;

                    if (lastItem != null)
                    {
                        var stdItem = OutlookClient.ConvertToStandardCalendarItem(item, null);
                        LogProcessingEvent(stdItem, "comparing ...");

                        if (lastItem.Subject == item.Subject
                            && lastItem.Start == item.Start
                            && lastItem.Body == item.Body &&
                            (item.Subject.StartsWith("Geburtstag", StringComparison.OrdinalIgnoreCase) ||
                             item.Subject.EndsWith("Birthday", StringComparison.OrdinalIgnoreCase)))
                        {
                            LogProcessingEvent(stdItem, "removing ...");

                            item.Delete();
                            continue;
                        }
                    }

                    lastItem = item;
                }
            }
            catch (System.Exception ex)
            {
                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "Error at name {0}: {1}", currentElementName, ex.Message));
            }
            finally
            {
                outlookNamespace.Logoff();
            }

            LogProcessingEvent("Remove duplicates finished");
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="element">the element to add</param>
        /// <param name="clientFolderName">the outlook folder to use</param>
        public override void AddItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="elements">list of all the elements to add</param>
        /// <param name="clientFolderName">the outlook folder to use</param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Implementation of the process of writing a single element and skipping this process if this 
        /// element is already present. If the element does not exist, it will be added. If it does exist
        /// the element will not be added and not be overridden.
        /// </summary>
        /// <param name="element">the element to be added</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public override void MergeMissingItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// Implementation of the process of writing a multiple elements by specifying a list of elements and 
        /// skipping this process if an element is already present. Missing elements will be added, existing 
        /// elements will not be altered.
        /// </summary>
        /// <param name="elements">the elements to be added in a list of elements</param>
        /// <param name="clientFolderName">the information where inside the source the elements reside - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        public override void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var currentElementName = string.Empty;
            var minimumDate = DateTime.Now;

            // get a connection to outlook 
            LogProcessingEvent("logging on ...");
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
                if (clientFolderName.Contains(":"))
                {
                    clientFolderName = clientFolderName.Substring(0, clientFolderName.IndexOf(":"));
                }

                // select a folder
                var outlookFolder = OutlookClient.GetOutlookMapiFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar);

                // if no folder has been selected, we will leave here
                if (outlookFolder == null)
                {
                    LogProcessingEvent("no outlook folder selected");
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
                                currentElementName = calendarStdItem.Start.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CurrentCulture) + " - " + calendarStdItem.Subject;

                                LogProcessingEvent("reading ... " + currentElementName);

                                result.Add(OutlookClient.ConvertToStandardCalendarItem(calendarStdItem, result.ToOtherType<StdElement, StdCalendarItem>()));
                            }
                        }
                        catch (System.Runtime.InteropServices.COMException ex)
                        {
                            if (ex.ErrorCode == -1285291755 ||
                                ex.ErrorCode == -2147221227)
                            {
                                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "problem accessing outlook store at name {0}: {1}", currentElementName, ex.Message));
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "Error at name {0}: {1}", currentElementName, ex.Message));
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
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "adding {0} elements ...", elements.Count));

            // create outlook instance and get the folder
            var outlookNamespace = OutlookClient.GetNamespace();
            var appointmentEnum = OutlookClient.GetOutlookMapiFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar).Items;

            // extract the contacts that do already exist
            var appointmentList = OutlookClient.GetAppointmentsList(appointmentEnum);

            var added = 0;
            foreach (var element in elements)
            {
                // find outlook contact with matching id, create new if needed
                LogProcessingEvent(element, "searching ...");
                if (OutlookClient.WriteCalendarItemToOutlook(appointmentEnum, (StdCalendarItem)element, appointmentList))
                {
                    added++;
                }
            }

            outlookNamespace.Logoff();
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "{0} elements added", added));
        }
    }
}