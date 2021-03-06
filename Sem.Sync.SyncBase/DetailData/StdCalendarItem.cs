﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdCalendarItem.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   defines a calendar entry which exists at a certain point in time
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// defines a calendar entry which exists at a certain point in time
    /// </summary>
    public class StdCalendarItem : StdElement
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StdCalendarItem" /> class.
        /// </summary>
        public StdCalendarItem()
        {
            if (this.ExternalIdentifier == null)
            {
                this.ExternalIdentifier = new ProfileIdentifierDictionary();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the <see cref = "BusyStatus" /> of the owner/related person.
        /// </summary>
        public BusyStatus BusyStatus { get; set; }

        /// <summary>
        ///   Gets or sets the body description (usually more text and more details than <see cref = "Title" />).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets the end date and time in UCT.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        ///   Gets or sets the location name of the event.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///   Gets or sets the ids of the attendees - a list of contacts that MIGHT attend.
        /// </summary>
        public List<MatchingEntry> OptionalAttendees { get; set; }

        /// <summary>
        ///   Gets or sets the Organizer of this event - the person who will get the response status.
        /// </summary>
        public MatchingEntry Organizer { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref = "RecurrenceState" /> describing whether this particular
        ///   "event" is a single "stand alone" event or a master/detail event of a series of event.
        /// </summary>
        public RecurrenceState RecurrenceState { get; set; }

        /// <summary>
        ///   Gets or sets the timespan when a riminder should be shown to the user.
        /// </summary>
        public TimeSpan ReminderBeforeStart { get; set; }

        /// <summary>
        ///   Gets or sets the ids of the required attendees - a list of contacts that MUST attend.
        /// </summary>
        public List<MatchingEntry> RequiredAttendees { get; set; }

        /// <summary>
        ///   Gets or sets the resources needed for this event.
        /// </summary>
        public List<string> Resources { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether a response is requested or not.
        ///   In case of a request to response, the <see cref = "ResponseStatus" /> should be different to <see cref = "DetailData.ResponseStatus.Undefined" />
        /// </summary>
        public bool ResponseRequested { get; set; }

        /// <summary>
        ///   Gets or sets ResponseStatus.
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }

        /// <summary>
        ///   Gets or sets the start date and time in UTC.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        ///   Gets or sets a "subject" or "title" describing the event.
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// normalizes the content of this entity in order to exclude leading/tailing spaces in strings etc.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override void NormalizeContent()
        {
            if (!string.IsNullOrEmpty(this.Description))
            {
                this.Description = this.Description.Trim();
            }

            if (!string.IsNullOrEmpty(this.Title))
            {
                this.Title = this.Title.Trim();
            }

            SyncTools.ClearNulls(this, typeof(StdCalendarItem));
        }

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>
        /// a meaningful string representation for this object
        /// </returns>
        public override string ToString()
        {
            return this.Start.ToString("yyyy-MM-dd hh:mm:ss - ", CultureInfo.InvariantCulture) + this.Title;
        }

        #endregion
    }
}