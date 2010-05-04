using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.Sync.Connector.Statistic
{
    using System.Globalization;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    public class StdContactResult
    {
        public static StdContactResult ValueAnalysisCounterStdContact(ICollection<StdContact> contacts)
        {
            if (contacts == null)
            {
                return null;
            }

            if (contacts.Count <= 0)
            {
                return null;
            }

            return new StdContactResult
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
                                               into g
                                               orderby g.Count() descending
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
                                               into g
                                               orderby g.Count() descending
                                               select
                                                   new KeyValuePair
                                                       {
                                                           Key = g.Key,
                                                           Value = g.Count().ToString(CultureInfo.CurrentCulture)
                                                       }).Take(10).
                        ToList()
                };
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
