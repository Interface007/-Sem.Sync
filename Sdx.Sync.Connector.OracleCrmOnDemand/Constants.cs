// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="SDX-AG">
//   (c) 2009 by SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Constants type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.GenericHelpers;

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines constants to work with Oracle CRM on Demand
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// List of information to query from the online system
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules",
            "SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine",
            Justification = "Reviewed. Suppression is OK here.")] 
        public static readonly Dictionary<string, string> PropertiesToQuery = new Dictionary<string, string>
                                    {
                                        {
                                            "ContactId", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "AccountName", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "CreatedDate", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "Department", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "Description", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "Gender", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "HomePhone", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "JobTitle", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "LastUpdated", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "MaritalStatus", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "MiddleName", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "ModifiedDate", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryAddress", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryCity", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryCountry", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryProvince", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryStateProvince", // todo: mapping
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryStreetAddress2", // todo: mapping
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryStreetAddress3", // todo: mapping
                                            "{emptystring}"
                                            },
                                        {
                                            "PrimaryZipCode", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "ContactLastName", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "ContactFirstName", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "ContactEmail", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "CellularPhone", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "WorkPhone", // mapped
                                            "{emptystring}"
                                            },
                                        {
                                            "DateofBirth", // mapped
                                            "{emptystring}"
                                            },
                                    };

        public static readonly List<string> PropertiesNotQueried = new List<string>();

        /// <summary>
        /// Initializes static members of the <see cref="Constants"/> class.
        /// </summary>
        static Constants()
        {
            var proplist = Tools.GetPropertyList(string.Empty, typeof(Contact));
            foreach (var propName in proplist)
            {
                if (Constants.PropertiesToQuery.ContainsKey(propName))
                {
                    continue;
                }

                PropertiesNotQueried.Add(propName);
            }
        }
    }
}