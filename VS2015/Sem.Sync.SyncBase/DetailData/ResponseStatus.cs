// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseStatus.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   response status of a calendar item
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// response status of a calendar item
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        ///   there is no defined status - the status is undefined
        /// </summary>
        Undefined = 0, 

        /// <summary>
        ///   the person did not respond
        /// </summary>
        NotResponded = 1, 

        /// <summary>
        ///   the person did accept the appointment
        /// </summary>
        Accepted = 2, 

        /// <summary>
        ///   the person is not sure if the person can join the appointment, but it wants to join
        /// </summary>
        Tentative = 3, 

        /// <summary>
        ///   the person does not want to join the appointment
        /// </summary>
        Declined = 4, 

        /// <summary>
        ///   the person is the organizer of the appointment
        /// </summary>
        Organized = 5, 
    }
}