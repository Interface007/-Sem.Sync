// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdCalendarItems.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines a result set of calendar item list analysis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.AnalysisModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Defines a result set of calendar item list analysis.
    /// </summary>
    public class StdCalendarItems
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the number of calendar entries for the next 14 days (2 weeks).
        /// </summary>
        public int NextFourteenDays { get; set; }

        /// <summary>
        ///   Gets or sets the number of calendar entries for the next seven days.
        /// </summary>
        public int NextSevenDays { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the <see cref="StdCalendarItems"/> if the parameter
        ///   <paramref name="calendarItems"/> does contain a list of <see cref="StdCalendarItem"/> 
        ///   elements with more than zero entries. Returns null in case of a null reference for
        ///   the parameter or an empty list.
        /// </summary>
        /// <param name="calendarItems">
        /// The list of calendar items to be analyzed. 
        /// </param>
        /// <returns>
        /// a new instance of the analysis results entity or null 
        /// </returns>
        public static StdCalendarItems GetAnalysisItemResult(List<StdCalendarItem> calendarItems)
        {
            if (calendarItems == null)
            {
                return null;
            }

            if (calendarItems.Count <= 0)
            {
                return null;
            }

            return new StdCalendarItems
                {
                    NextSevenDays =
                        (from x in calendarItems
                         where x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(7)
                         select x).Count(), 
                    NextFourteenDays =
                        (from x in calendarItems
                         where x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(14)
                         select x).Count()
                };
        }

        #endregion
    }
}