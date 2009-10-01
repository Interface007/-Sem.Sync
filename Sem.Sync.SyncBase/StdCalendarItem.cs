//-----------------------------------------------------------------------
// <copyright file="StdCalendarItem.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
// The complete class is currently undecomuented, because the structure of the internal calendar
// item format is not clear at the memoment and will definitely change in the near future.
#pragma warning disable 1591
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DetailData;

    /// <summary>
    /// status of a calendar entry or a person
    /// </summary>
    public enum BusyStatus
    {
        /// <summary>
        /// the status is not known
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// the calendar item is not blocking other activities
        /// </summary>
        Free = 1,

        /// <summary>
        /// the calendar item is tentative
        /// </summary>
        Tentative = 2,

        /// <summary>
        /// the person is blocked from other activities
        /// </summary>
        Busy = 3,

        /// <summary>
        /// the person is out of office
        /// </summary>
        OutOfOffice = 4,
    }

    /// <summary>
    /// response status of a calendar item
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// there is no defined status - the status is undefined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// the person did not respond
        /// </summary>
        NotResponded = 1,

        /// <summary>
        /// the person did accept the appointment
        /// </summary>
        Accepted = 2,

        /// <summary>
        /// the person is not sure if the person can join the appointment, but it wants to join
        /// </summary>
        Tentative = 3,
        
        /// <summary>
        /// the person does not want to join the appointment
        /// </summary>
        Declined = 4,

        /// <summary>
        /// the person is the organizer of the appointment
        /// </summary>
        Organized = 5,
    }

    /// <summary>
    /// defines the way of reoccurance of the appointment
    /// </summary>
    public enum RecurrenceState
    {
        /// <summary>
        /// one time only event
        /// </summary>
        Onetime = 0,

        /// <summary>
        /// starting point of a series of events
        /// </summary>
        Master = 1,

        /// <summary>
        /// single occurance of a series of events, that does inherit all from the master
        /// </summary>
        Occurrence = 2,

        /// <summary>
        /// exceptional event of a series of events
        /// </summary>
        Exception = 3,
    }

    /// <summary>
    /// defines a calendar entry which exists at a certain point in time
    /// </summary>
    public class StdCalendarItem : StdElement
    {        
        #region _AppointmentItem Members

        public string EntryId { get; set; }
        public string GlobalAppointmentId { get; set; }
        public DateTime LastModificationTime { get; set; }
        
        public RecurrenceState RecurrenceState { get; set; }
        public BusyStatus BusyStatus { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string TimeZone { get; set; }
        
        public bool ResponseRequested { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public string Subject { get; set; }
        public string Location { get; set; }
        public string Body { get; set; }
        
        public string Categories { get; set; }
        
        public string Links { get; set; }
        
        public string RequiredAttendees { get; set; }
        public string OptionalAttendees { get; set; }
        public string Resources { get; set; }
        
        public string Organizer { get; set; }
        public string Recipients { get; set; }
        
        public int ReminderMinutesBeforeStart { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the list of ExternalIdentifier.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "serialization")]
        public List<CalendarIdentifier> ExternalIdentifier { get; set; }

        public StdCalendarItem()
        {
            this.ExternalIdentifier = new List<CalendarIdentifier>();
        }

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>a meaningful string representation for this object</returns>
        public override string ToString()
        {
            return this.Start.ToString("yyyy-MM-dd hh:mm:ss - ", CultureInfo.InvariantCulture) + this.Subject;
        }

        public override void NormalizeContent()
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore 1591
}