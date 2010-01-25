// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessHistoryEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class describes a time span in the business carreer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// This class describes a time span in the business carreer
    /// </summary>
    public class BusinessHistoryEntry
    {
        /// <summary>
        /// Gets or sets the Business Company Name.
        /// </summary>
        public string BusinessCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Business Department.
        /// </summary>
        public string BusinessDepartment { get; set; }

        /// <summary>
        /// Gets or sets the name of the Business Position.
        /// </summary>
        public string BusinessPosition { get; set; }

        /// <summary>
        /// Gets or sets the Start date of this timespan.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the End date of this timespan.
        /// </summary>
        public DateTime End { get; set; }
    }
}
