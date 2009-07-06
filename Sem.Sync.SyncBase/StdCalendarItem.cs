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

    public enum BusyStatus
    {
        Undefined = 0,
        Free = 1,
        Tentative = 2,
        Busy = 3,
        OutOfOffice = 4,
    }

    public enum ResponseStatus
    {
        Undefined = 0,
        NotResponded = 1,
        Accepted = 2,
        Tentative = 3,
        Declined = 4,
        Organized = 5,
    }

    public enum RecurrenceState
    {
        Onetime = 0,
        Master = 1,
        Occurrence = 2,
        Exception = 3,
    }

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