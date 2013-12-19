// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecurrenceState.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   defines the way of reoccurance of the appointment
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// defines the way of reoccurance of the appointment
    /// </summary>
    public enum RecurrenceState
    {
        /// <summary>
        ///   one time only event
        /// </summary>
        Onetime = 0, 

        /// <summary>
        ///   starting point of a series of events
        /// </summary>
        Master = 1, 

        /// <summary>
        ///   single occurance of a series of events, that does inherit all from the master
        /// </summary>
        Occurrence = 2, 

        /// <summary>
        ///   exceptional event of a series of events
        /// </summary>
        Exception = 3, 
    }
}