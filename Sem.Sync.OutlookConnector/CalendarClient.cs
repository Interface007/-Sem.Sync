// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling outlook calendar items
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OutlookConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.Office.Interop.Outlook;

    using SyncBase;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling outlook calendar items
    /// </summary>
    public class CalendarClient : StdClient
    {
        /// <summary>
        /// Gets the ui friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "Outlook-Canlendar-Connector"; }
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

                _AppointmentItem lastItem = null;
                foreach (var item in outlookItemList)
                {
                    currentElementName = item.Subject;

                    if (lastItem != null)
                    {
                        var stdItem = OutlookClient.ConvertToStandardCalendarItem(item);
                        LogProcessingEvent(stdItem, "comparing ...");

                        if (lastItem.Subject == item.Subject
                            && lastItem.Start == item.Start
                            && lastItem.Body == item.Body
                            && item.Subject.StartsWith("Geburtstag", StringComparison.OrdinalIgnoreCase))
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

        public override void MergeMissingItem(StdElement element, string clientFolderName)
        {
            var elements = new List<StdElement> { element };
            this.WriteFullList(elements, clientFolderName, true);
        }

        public override void MergeMissingRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var currentElementName = string.Empty;

            // get a connection to outlook 
            LogProcessingEvent("logging on ...");
            var outlookNamespace = OutlookClient.GetNamespace();

            // we need to log off from outlook in order to clean up the session
            try
            {
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
                            if (calendarStdItem != null)
                            {
                                currentElementName = calendarStdItem.Start.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CurrentCulture) + " - " + calendarStdItem.Subject;

                                LogProcessingEvent("reading ... " + currentElementName);

                                result.Add(OutlookClient.ConvertToStandardCalendarItem(calendarStdItem));
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

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, "adding {0} elements ...", elements.Count));

            // create outlook instance and get the folder
            var outlookNamespace = OutlookClient.GetNamespace();
            var appointmentEnum = OutlookClient.GetOutlookMapiFolder(outlookNamespace, clientFolderName, OlDefaultFolders.olFolderCalendar).Items;

            // extract the contacts that do already exist
            var appointmentList = OutlookClient.GetContactsList(appointmentEnum);

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