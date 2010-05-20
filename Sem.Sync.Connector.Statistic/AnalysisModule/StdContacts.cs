// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdContacts.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Calculates and represents some statistic information about the contacts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.AnalysisModule
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Calculates and represents some statistic information about the contacts.
    /// </summary>
    public class StdContacts
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the amount of entries specifying the same business company property.
        /// </summary>
        public List<KeyValuePair> BusinessCompanies { get; set; }

        /// <summary>
        ///   Gets or sets the percentage of contacts that do have a specified gender "female".
        /// </summary>
        public decimal PercentageGenderFemale { get; set; }

        /// <summary>
        ///   Gets or sets the percentage of contacts that do have a specified gender "female".
        /// </summary>
        public decimal PercentageGenderMale { get; set; }

        /// <summary>
        ///   Gets or sets the top 10 used city names in personal address.
        /// </summary>
        public List<KeyValuePair> Top10CitiesBusiness { get; set; }

        /// <summary>
        ///   Gets or sets the top 10 used city names in personal address.
        /// </summary>
        public List<KeyValuePair> Top10CitiesPersonal { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of the <see cref="StdContacts"/> if the parameter
        ///   <paramref name="contacts"/> does contain a list of <see cref="StdContact"/> 
        ///   elements with more than zero entries. Returns null in case of a null reference for
        ///   the parameter or an empty list.
        /// </summary>
        /// <param name="contacts">
        /// The list of contact items to be analyzed. 
        /// </param>
        /// <returns>
        /// a new instance of the analysis results entity or null 
        /// </returns>
        public static StdContacts GetAnalysisItemResult(ICollection<StdContact> contacts)
        {
            if (contacts == null)
            {
                return null;
            }

            if (contacts.Count <= 0)
            {
                return null;
            }

            return new StdContacts
                {
                    PercentageGenderMale =
                        Math.Round(
                            (decimal)((from x in contacts where x.PersonGender == Gender.Male select x).Count() * 100) /
                            contacts.Count, 
                            2), 
                    PercentageGenderFemale =
                        Math.Round(
                            (decimal)((from x in contacts where x.PersonGender == Gender.Female select x).Count() * 100) /
                            contacts.Count, 
                            2), 
                    Top10CitiesPersonal = (from x in contacts
                                           where
                                               x.PersonalAddressPrimary != null &&
                                               !string.IsNullOrEmpty(x.PersonalAddressPrimary.CityName)
                                           group x by x.PersonalAddressPrimary.CityName
                                           into g orderby g.Count() descending
                                           select
                                               new KeyValuePair
                                                   {
                                                       Key = g.Key, 
                                                       Value = g.Count().ToString(CultureInfo.CurrentCulture)
                                                   }).Take(10).
                        ToList(), 
                    Top10CitiesBusiness = (from x in contacts
                                           where
                                               x.BusinessAddressPrimary != null &&
                                               !string.IsNullOrEmpty(x.BusinessAddressPrimary.CityName)
                                           group x by x.BusinessAddressPrimary.CityName
                                           into g orderby g.Count() descending
                                           select
                                               new KeyValuePair
                                                   {
                                                       Key = g.Key, 
                                                       Value = g.Count().ToString(CultureInfo.CurrentCulture)
                                                   }).Take(10).
                        ToList(), 
                    BusinessCompanies = (from x in contacts
                                         where !string.IsNullOrEmpty(x.BusinessCompanyName)
                                         group x by x.BusinessCompanyName
                                         into g orderby g.Count() descending
                                         select
                                             new KeyValuePair
                                                 {
                                                    Key = g.Key, Value = g.Count().ToString(CultureInfo.CurrentCulture) 
                                                 })
                        .ToList()
                };
        }

        #endregion
    }
}