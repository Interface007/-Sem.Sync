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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using GenericHelpers.Entities;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    /// <summary>
    /// Implements some of the statistical information.
    /// </summary>
    public class ValueAnalysisCounter
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
        public ValueAnalysisCounter(List<StdElement> elements)
        {
            var contacts = elements.ToStdContacts();

            if (contacts.Count > 0)
            {
                this.PercentageGenderMale = Math.Round((decimal)((from x in contacts where x.PersonGender == Gender.Male select x).Count() * 100) / contacts.Count, 2);
                this.PercentageGenderFemale = Math.Round((decimal)((from x in contacts where x.PersonGender == Gender.Female select x).Count() * 100) / contacts.Count, 2);

                this.Top10CitiesPersonal = (from x in contacts
                                            where x.PersonalAddressPrimary != null && !string.IsNullOrEmpty(x.PersonalAddressPrimary.CityName)
                                            group x by x.PersonalAddressPrimary.CityName
                                                into g
                                                orderby g.Count() descending
                                                select
                                                    new KeyValuePair
                                                        {
                                                            Key = g.Key,
                                                            Value = g.Count().ToString(CultureInfo.CurrentCulture)
                                                        }).Take(10).ToList();

                this.Top10CitiesBusiness = (from x in contacts
                                            where x.BusinessAddressPrimary != null && !string.IsNullOrEmpty(x.BusinessAddressPrimary.CityName)
                                            group x by x.BusinessAddressPrimary.CityName
                                                into g
                                                orderby g.Count() descending
                                                select
                                                    new KeyValuePair
                                                        {
                                                            Key = g.Key,
                                                            Value = g.Count().ToString(CultureInfo.CurrentCulture)
                                                        }).Take(10).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the percentage of contacts that do have a specified gender "female".
        /// </summary>
        public decimal PercentageGenderFemale { get; set; }

        /// <summary>
        /// Gets or sets the percentage of contacts that do have a specified gender "female".
        /// </summary>
        public decimal PercentageGenderMale { get; set; }

        /// <summary>
        /// Gets or sets the top 10 used city names in personal address.
        /// </summary>
        public List<KeyValuePair> Top10CitiesPersonal { get; set; }

        /// <summary>
        /// Gets or sets the top 10 used city names in personal address.
        /// </summary>
        public List<KeyValuePair> Top10CitiesBusiness { get; set; }
    }
}