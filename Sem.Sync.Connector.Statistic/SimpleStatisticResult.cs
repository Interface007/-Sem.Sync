// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleStatisticResult.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines a result set for the simple statistic
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.Connector.Statistic.AnalysisModule;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Defines a result set for the simple statistic
    /// </summary>
    [XmlInclude(typeof(StdCalendarItems))]
    [XmlInclude(typeof(StdContacts))]
    [XmlInclude(typeof(List<KeyValuePair>))]
    public class SimpleStatisticResult
    {
        #region Constants and Fields

        /// <summary>
        ///   the internal array list to store the result sets
        /// </summary>
        private readonly ArrayList internalItems = new ArrayList();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SimpleStatisticResult" /> class.
        ///   The default constructor is to support XML serialization.
        /// </summary>
        public SimpleStatisticResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleStatisticResult"/> class and populates
        ///   the internal list with results from the analysis modules.
        /// </summary>
        /// <param name="elements">
        /// The elements. 
        /// </param>
        public SimpleStatisticResult(List<StdElement> elements)
        {
            this.NumberOfElements = elements.Count;
            this.AddItem(PropertyUsage.GetAnalysisItemResult(elements));
            this.AddItem(StdCalendarItems.GetAnalysisItemResult(elements.ToStdCalendarItems()));
            this.AddItem(StdContacts.GetAnalysisItemResult(elements.ToStdContacts()));
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets result elements.
        /// </summary>
        [XmlElement("AnalysisResult")]
        public object[] AnalysisResults
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
        ///   Gets or sets the number of elements.
        /// </summary>
        public int NumberOfElements { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new item to the result list. NULL values will be ignored.
        /// </summary>
        /// <param name="item">
        /// the item to be added
        /// </param>
        /// <returns>
        /// the index of the new item - -1 in case of a NULL item that has not been added
        /// </returns>
        public int AddItem(object item)
        {
            return item != null ? this.internalItems.Add(item) : -1;
        }

        #endregion
    }
}