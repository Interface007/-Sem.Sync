// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="SDX-AG">
//   (c) 2009 by SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Converter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Globalization;
    
    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using System.Collections.Generic;

    /// <summary>
    /// Converter class to convert native to std format
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converts a Oracle CRM on Demand <see cref="Contact"/> to a <see cref="StdContact"/>
        /// </summary>
        /// <param name="contact"> The Oracle CRM on Demand contact. </param>
        /// <returns> the converted data as a <see cref="StdContact"/> </returns>
        public static StdContact ToStdContact(this Contact contact)
        {
            return ToStdContact(contact, false);
        }

        /// <summary>
        /// Converts a Oracle CRM on Demand <see cref="Contact"/> to a <see cref="StdContact"/>
        /// </summary>
        /// <param name="contact"> The Oracle CRM on Demand contact. </param>
        /// <param name="addCustomAttributes"> a value specifying whether non-mapped properties should be added to the <see cref="StdContact.SourceSpecificAttributes"/>. </param>
        /// <returns> the converted data as a <see cref="StdContact"/> </returns>
        public static StdContact ToStdContact(this Contact contact, bool addCustomAttributes)
        {
            var result = new StdContact
                             {
                                 InternalSyncData = new SyncData
                                                        {
                                                            DateOfCreation = TryParseDateTime(contact.CreatedDate),
                                                            DateOfLastChange =
                                                                TryParseDateTime(
                                                                string.IsNullOrEmpty(contact.ModifiedDate)
                                                                    ? contact.LastUpdated
                                                                    : contact.ModifiedDate),
                                                        },
                                 AdditionalTextData = contact.Description,
                                 BusinessAddressPrimary = new AddressDetail
                                 {
                                     Phone = new PhoneNumber(contact.WorkPhone),
                                     CityName = contact.PrimaryCity,
                                     PostalCode = contact.PrimaryZipCode,
                                     CountryName = contact.PrimaryCountry,
                                     StateName = contact.PrimaryProvince,
                                 },
                                 PersonalAddressPrimary = new AddressDetail
                                 {
                                     Phone = new PhoneNumber(contact.HomePhone),
                                     ////CityName = contact.PrimaryCity,
                                     ////PostalCode = contact.PrimaryZipCode,
                                     ////CountryName = contact.PrimaryCountry,
                                     ////StateName = contact.PrimaryProvince,
                                 },
                                 BusinessEmailPrimary = contact.ContactEmail,
                                 BusinessPhoneMobile = new PhoneNumber(contact.CellularPhone),
                                 BusinessPosition = contact.JobTitle,
                                 BusinessDepartment = contact.Department,
                                 BusinessCompanyName = contact.AccountName,
                                 
                                 // todo: check the values here! "male" and "female" are just guesses
                                 PersonGender =
                                     contact.Gender == "male"
                                         ? Gender.Male
                                         : contact.Gender == "female"
                                               ? Gender.Female
                                               : Gender.Unspecified,
                                 
                                 // todo: check the values here! "male" and "female" are just guesses
                                 RelationshipStatus = contact.MaritalStatus == "married" ? RelationshipStatus.Married : RelationshipStatus.Undefined,
                                 DateOfBirth = TryParseDateTime(contact.DateofBirth),
                                 Name = new PersonName
                                            {
                                                FirstName = contact.ContactFirstName,
                                                MiddleName = contact.MiddleName,
                                                LastName = contact.ContactLastName,
                                            },
                                 PersonalProfileIdentifiers = { OracleCrmOnDemandId = contact.ContactId },
                             };

            Tools.DebugWriteLine(!string.IsNullOrEmpty(contact.MaritalStatus), "contact.MaritalStatus = {0}", contact.MaritalStatus);
            Tools.DebugWriteLine(!string.IsNullOrEmpty(contact.Gender), "contact.Gender = {0}", contact.Gender);

            if (addCustomAttributes)
            {
                result.SourceSpecificAttributes = new List<SourceSpecificAttribute>();
                var sourceConnectorName = typeof (ContactClient).FullName;
                foreach (var x in Constants.PropertiesNotQueried)
                {
                    var value = Tools.GetPropertyValueString(contact, x);

                    if (!string.IsNullOrEmpty(value))
                    {
                        result.SourceSpecificAttributes.Add(
                        new SourceSpecificAttribute
                            {
                                AttributeName = x,
                                AttributeValue = value,
                                SourceConnector = sourceConnectorName
                            });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a datetime and returns the default instance of a DateTime object of parsing was not possible.
        /// </summary>
        /// <param name="datetime"> The datetime as a string to be parsed. </param>
        /// <returns> the parsed datetime if it was possible to parse the string, a new DateTime object, if not </returns>
        private static DateTime TryParseDateTime(string datetime)
        {
            DateTime result;
            return DateTime.TryParse(datetime, CultureInfo.InvariantCulture, DateTimeStyles.None, out result) 
                       ? result 
                       : new DateTime();
        }
    }
}