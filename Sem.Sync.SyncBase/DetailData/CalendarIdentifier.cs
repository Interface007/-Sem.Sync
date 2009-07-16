﻿namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Identifies the calendar provider/type
    /// </summary>
    public enum CalendarIdentifierType
    {
        /// <summary>
        /// Unspecific calendar - not registered as a known type
        /// </summary>
        Default = 0,

        /// <summary>
        /// Microsoft Outlook calendar
        /// </summary>
        Outlook,

        /// <summary>
        /// Google Calendar
        /// </summary>
        Google,
    }
    
    /// <summary>
    /// Identifier that describes the calendar (type and name) of the calendar
    /// </summary>
    public class CalendarIdentifier
    {
        /// <summary>
        /// Gets or sets the type of the calendar - different types need different connectors
        /// </summary>
        public CalendarIdentifierType IdentifierType { get; set; }

        /// <summary>
        /// Gets or sets the "real" identifier of the calendar. This is only unique within one type of 
        /// calendars and should contain the information that uniquely identifies the calendar instance.
        /// </summary>
        public string Identifier { get; set; }
    }
}
