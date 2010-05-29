// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusyStatus.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   status of a calendar entry or a person
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// status of a calendar entry or a person
    /// </summary>
    public enum BusyStatus
    {
        /// <summary>
        ///   the status is not known
        /// </summary>
        Undefined = 0, 

        /// <summary>
        ///   the calendar item is not blocking other activities
        /// </summary>
        Free = 1, 

        /// <summary>
        ///   the calendar item is tentative
        /// </summary>
        Tentative = 2, 

        /// <summary>
        ///   the person is blocked from other activities (not accepting new tasks)
        /// </summary>
        Busy = 3, 

        /// <summary>
        ///   the person is out of office (not event able to accept new tasks in the office)
        /// </summary>
        OutOfOffice = 4, 
    }
}