// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleStatisticResult.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the SimpleStatisticResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections.Generic;

    using GenericHelpers.Entities;

    /// <summary>
    /// Defines a result set for the simple statistic
    /// </summary>
    public class SimpleStatisticResult
    {
        /// <summary>
        /// Gets or sets the number of elements.
        /// </summary>
        public int NumberOfElements { get; set; }

        /// <summary>
        /// Gets or sets the number of usages for each property.
        /// </summary>
        public List<KeyValuePair> PropertyUsage { get; set; }

        /// <summary>
        /// Gets or sets value analysis results.
        /// </summary>
        public ValueAnalysisCounter ValueAnalysis { get; set; }
    }
}
