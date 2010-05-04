// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueAnalysisCounter.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Implements some of the statistical information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using SyncBase;
    using SyncBase.Helpers;

    /// <summary>
    /// Implements some of the statistical information.
    /// </summary>
    [XmlInclude(typeof(StdCalendarItemResult))]
    [XmlInclude(typeof(StdContactResult))]
    public class ValueAnalysisCounter : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAnalysisCounter"/> class.
        /// </summary>
        public ValueAnalysisCounter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueAnalysisCounter"/> class.
        /// </summary>
        /// <param name="elements">
        /// The elements to be analyzed.
        /// </param>
        public ValueAnalysisCounter(IEnumerable<StdElement> elements)
        {
            
            this.Add(StdCalendarItemResult.GetStdCalendarItemResult(elements.ToStdCalendarItems()));
            this.Add(StdContactResult.ValueAnalysisCounterStdContact(elements.ToStdContacts()));
        }
    }
}