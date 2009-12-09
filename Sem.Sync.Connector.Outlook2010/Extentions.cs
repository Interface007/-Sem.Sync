namespace Sem.Sync.Connector.Outlook2010
{
    using System;

    using Microsoft.Office.Interop.Outlook;

    using Sem.Sync.SyncBase;

    public static class Extentions
    {
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