// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarIdentifier.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Identifies the calendar provider/type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

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
    [Serializable]
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