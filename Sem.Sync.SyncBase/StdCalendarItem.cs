namespace Sem.Sync.SyncBase
{
    using System;
    using System.Globalization;

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
        public DateTime CreationTime { get; set; }
        public DateTime LastModificationTime { get; set; }
        
        public bool IsAllDayEvent { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceState RecurrenceState { get; set; }
        public bool ResponseRequested { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public string Subject { get; set; }
        public string Location { get; set; }
        public string Body { get; set; }
        
        public string Categories { get; set; }
        
        public int Duration { get; set; }
        public DateTime EndUtc { get; set; }
        public DateTime StartUtc { get; set; }
        public string StartTimeZone { get; set; }

        public BusyStatus BusyStatus { get; set; }

        public string Links { get; set; }
        
        public bool NoAging { get; set; }

        public string RequiredAttendees { get; set; }
        public string OptionalAttendees { get; set; }
        public string Resources { get; set; }
        
        public string Organizer { get; set; }
        public string Recipients { get; set; }
        
        public int ReminderMinutesBeforeStart { get; set; }
        public bool ReminderOverrideDefault { get; set; }
        public bool ReminderPlaySound { get; set; }
        public bool ReminderSet { get; set; }
        public string ReminderSoundFile { get; set; }
        
        #endregion

        public override string ToString()
        {
            return this.StartUtc.ToString("yyyy-MM-dd hh:mm:ss - ", CultureInfo.InvariantCulture) + this.Subject;
        }

        public override void NormalizeContent()
        {
            throw new NotImplementedException();
        }
    }

}

