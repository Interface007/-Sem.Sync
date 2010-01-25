// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook2010
{
    using System;

    using Microsoft.Office.Interop.Outlook;

    using Sem.Sync.SyncBase;

    /// <summary>
    /// Static helper class to convert entities
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Translates a <see cref="OlBusyStatus"/> into a <see cref="BusyStatus"/>.
        /// </summary>
        /// <param name="status"> The <see cref="OlBusyStatus"/> to translate into a <see cref="BusyStatus"/>. </param>
        /// <returns>
        /// The corresponding <see cref="BusyStatus"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">In case of an unknown <see cref="OlBusyStatus"/>. </exception>
        public static BusyStatus ToBusyStatus(this OlBusyStatus status)
        {
            switch (status)
            {
                case OlBusyStatus.olFree:
                    return BusyStatus.Free;
                case OlBusyStatus.olBusy:
                    return BusyStatus.Busy;
                case OlBusyStatus.olOutOfOffice:
                    return BusyStatus.OutOfOffice;
                case OlBusyStatus.olTentative:
                    return BusyStatus.Tentative;
                default:
                    throw new ArgumentOutOfRangeException(
                        "status", 
                        status, 
                        "there is no translation for this value into the SemSync namespace.");
            }
        }

        public static OlRecurrenceState ToOutlook(this RecurrenceState recurrence)
        {
            switch (recurrence)
            {
                case RecurrenceState.Onetime:
                    return OlRecurrenceState.olApptNotRecurring;
                    
                case RecurrenceState.Master:
                    return OlRecurrenceState.olApptMaster;

                case RecurrenceState.Occurrence:
                    return OlRecurrenceState.olApptOccurrence;

                case RecurrenceState.Exception:
                    return OlRecurrenceState.olApptException;

                default:
                    throw new ArgumentOutOfRangeException("recurrence");
            }
        }

        public static OlResponseStatus ToOutlook(this ResponseStatus status)
        {
            switch (status)
            {
                case ResponseStatus.Undefined:
                    return OlResponseStatus.olResponseNone;

                case ResponseStatus.NotResponded:
                    return OlResponseStatus.olResponseNotResponded;

                case ResponseStatus.Accepted:
                    return OlResponseStatus.olResponseAccepted;

                case ResponseStatus.Tentative:
                    return OlResponseStatus.olResponseTentative;

                case ResponseStatus.Declined:
                    return OlResponseStatus.olResponseDeclined;

                case ResponseStatus.Organized:
                    return OlResponseStatus.olResponseOrganized;

                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }

        public static OlBusyStatus ToOutlook(this BusyStatus status)
        {
            switch (status)
            {
                case BusyStatus.Free:
                    return OlBusyStatus.olFree;
                case BusyStatus.Busy:
                    return OlBusyStatus.olBusy;
                case BusyStatus.OutOfOffice:
                    return OlBusyStatus.olOutOfOffice;
                case BusyStatus.Tentative:
                    return OlBusyStatus.olTentative;
                default:
                    throw new ArgumentOutOfRangeException(
                        "status",
                        status,
                        "there is no translation for this value into the SemSync namespace.");
            }
        }

        /// <summary>
        /// Translates a <see cref="OlRecurrenceState"/> into a <see cref="RecurrenceState"/>.
        /// </summary>
        /// <param name="status"> The <see cref="OlRecurrenceState"/> to translate into a <see cref="RecurrenceState"/>. </param>
        /// <returns>
        /// The corresponding <see cref="RecurrenceState"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">In case of an unknown <see cref="OlRecurrenceState"/>. </exception>
        public static RecurrenceState ToRecurrenceState(this OlRecurrenceState status)
        {
            switch (status)
            {
                case OlRecurrenceState.olApptNotRecurring:
                    return RecurrenceState.Onetime;

                case OlRecurrenceState.olApptMaster:
                    return RecurrenceState.Master;

                case OlRecurrenceState.olApptOccurrence:
                    return RecurrenceState.Occurrence;

                case OlRecurrenceState.olApptException:
                    return RecurrenceState.Exception;

                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }

        /// <summary>
        /// Translates a <see cref="OlResponseStatus"/> into a <see cref="ResponseStatus"/>.
        /// </summary>
        /// <param name="status"> The <see cref="OlResponseStatus"/> to translate into a <see cref="ResponseStatus"/>. </param>
        /// <returns>
        /// The corresponding <see cref="ResponseStatus"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">In case of an unknown <see cref="OlResponseStatus"/>. </exception>
        public static ResponseStatus ToResponseStatus(this OlResponseStatus status)
        {
            switch (status)
            {
                case OlResponseStatus.olResponseNone:
                    return ResponseStatus.Undefined;

                case OlResponseStatus.olResponseOrganized:
                    return ResponseStatus.Organized;

                case OlResponseStatus.olResponseTentative:
                    return ResponseStatus.Tentative;

                case OlResponseStatus.olResponseAccepted:
                    return ResponseStatus.Accepted;

                case OlResponseStatus.olResponseDeclined:
                    return ResponseStatus.Declined;

                case OlResponseStatus.olResponseNotResponded:
                    return ResponseStatus.NotResponded;

                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }
    }
}