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
    /// Implements some of the statistical information.
    /// </summary>
    [XmlInclude(typeof(StdCalendarItemResult))]
    [XmlInclude(typeof(StdContactResult))]
    public class ValueAnalysisCounter
    {
        private ArrayList internalItems = new ArrayList();

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

        public int AddItem(object item)
        {
            return 
                item != null
                ? this.internalItems.Add(item) 
                : -1;
        }
    }
}