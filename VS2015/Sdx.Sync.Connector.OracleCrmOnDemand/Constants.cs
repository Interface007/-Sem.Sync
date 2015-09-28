// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Constants type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Sdx.Sync.Connector.OracleCrmOnDemand.ContactSR;

    using Sem.GenericHelpers;

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
        public static readonly List<string> PropertiesToQueryForContacts = new List<string>
                                    {
                                            "ContactId", // mapped
                                            "AccountName", // mapped
                                            "CreatedDate", // mapped
                                            "Department", // mapped
                                            "Description", // mapped
                                            "Gender", // mapped
                                            "HomePhone", // mapped
                                            "JobTitle", // mapped
                                            "LastUpdated", // mapped
                                            "MaritalStatus", // mapped
                                            "MiddleName", // mapped
                                            "ModifiedDate", // mapped
                                            "PrimaryAddress", // mapped
                                            "PrimaryCity", // mapped
                                            "PrimaryCountry", // mapped
                                            "PrimaryProvince", // mapped
                                            "PrimaryStateProvince", // todo: mapping
                                            "PrimaryStreetAddress2", // todo: mapping
                                            "PrimaryStreetAddress3", // todo: mapping
                                            "PrimaryZipCode", // mapped
                                            "ContactLastName", // mapped
                                            "ContactFirstName", // mapped
                                            "ContactEmail", // mapped
                                            "CellularPhone", // mapped
                                            "WorkPhone", // mapped
                                            "DateofBirth", // mapped
                                    };

        /// <summary>
        /// Properties of contacts that are not mapped
        /// </summary>
        public static readonly List<string> PropertiesNotMapped = new List<string>();

        /// <summary>
        /// Initializes static members of the <see cref="Constants"/> class.
        /// </summary>
        static Constants()
        {
            var proplist = Tools.GetPropertyList(string.Empty, typeof(ContactData));
            foreach (var propName in proplist)
            {
                if (PropertiesToQueryForContacts.Contains(propName) 
                    || propName.EndsWith("Specified", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                PropertiesNotMapped.Add(propName);
            }

            PropertiesNotMapped.Sort();
        }
    }
}