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
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using SyncBase;
    using SyncBase.Helpers;

    /// <summary>
    /// Implements a list of statistical information results.
    /// </summary>
    [XmlInclude(typeof(StdCalendarItemResult))]
    [XmlInclude(typeof(StdContactResult))]
    public class ValueAnalysisCounter
    {
        private readonly ArrayList internalItems = new ArrayList();

        /// <summary>
        /// Creates a new instance of the <see cref="ValueAnalysisCounter"/> class.
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
            this.AddItem(StdCalendarItemResult.GetStdCalendarItemResult(elements.ToStdCalendarItems()));
            this.AddItem(StdContactResult.ValueAnalysisCounterStdContact(elements.ToStdContacts()));
        }
        
        /// <summary>
        /// Indexer to access the result elements.
        /// </summary>
        [XmlElement("AnalysisResult")]
        public object[] AnalysiResults
        {
            get
            {
                var items = new object[this.internalItems.Count];
                this.internalItems.CopyTo(items);
                return items;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                var items = value;
                this.internalItems.Clear();
                foreach (var item in items)
                {
                    this.internalItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds a new item to the result list. NULL values will be ignored.
        /// </summary>
        /// <param name="item">the item to be added</param>
        /// <returns>the index of the new item - -1 in case of a NULL item that has not been added</returns>
        public int AddItem(object item)
        {
            return 
                item != null
                ? this.internalItems.Add(item) 
                : -1;
        }
    }
}