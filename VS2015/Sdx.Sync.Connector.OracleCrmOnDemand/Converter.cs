// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="SDX-AG">
//   (c) 2010 by SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Converter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Linq;

    using Sdx.Sync.Connector.OracleCrmOnDemand.ContactSR;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// Converter class to convert native to std format
    /// </summary>
    internal static class Converter
    {
        /// <summary>
        /// Converts a Oracle CRM on Demand <see cref="ContactData"/> to a <see cref="StdContact"/>
        /// </summary>
        /// <param name="contact"> The Oracle CRM on Demand contact. </param>
        /// <param name="addCustomAttributes"> a value specifying whether non-mapped properties should be added to the <see cref="StdContact.SourceSpecificAttributes"/>. </param>
        /// <returns> the converted data as a <see cref="StdContact"/> </returns>
        internal static StdContact ToStdContact(this ContactData contact, bool addCustomAttributes)
        {
            var result = new StdContact
                             {
                                 InternalSyncData = new SyncData
                                                        {
                                                            DateOfCreation = contact.CreatedDate,
                                                            DateOfLastChange = contact.ModifiedDate,
                                                        },
                                 AdditionalTextData = contact.Description,
                                 BusinessAddressPrimary = new AddressDetail
                                 {
                                     Phone = new PhoneNumber(contact.WorkPhone),
                                     CityName = contact.PrimaryCity,
                                     PostalCode = contact.PrimaryZipCode,
                                     CountryName = contact.PrimaryCountry,
                                     StreetName = contact.PrimaryAddress,
                                     StateName = contact.PrimaryProvince,
                                 },
                                 PersonalAddressPrimary = new AddressDetail
                                 {
                                     Phone = new PhoneNumber(contact.HomePhone),
                                     CityName = contact.PrimaryCity,
                                     PostalCode = contact.PrimaryZipCode,
                                     CountryName = contact.PrimaryCountry,
                                     StreetName = contact.PrimaryAddress,
                                     StateName = contact.PrimaryProvince,
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

                                 // todo: check the values here! "married" and "other" are just guesses
                                 RelationshipStatus = contact.MaritalStatus == "married" ? RelationshipStatus.Married : RelationshipStatus.Undefined,
                                 DateOfBirth = contact.DateofBirth,
                                 Name = new PersonName
                                            {
                                                AcademicTitle = contact.MrMrs,
                                                FirstName = contact.ContactFirstName,
                                                MiddleName = contact.MiddleName,
                                                LastName = contact.ContactLastName,
                                            },
                                 ExternalIdentifier = new ProfileIdentifierDictionary(ProfileIdentifierType.OracleCrmOnDemandId, contact.Id),
                             };

            // we may need to collect the "other" properties
            if (addCustomAttributes)
            {
                result.SourceSpecificAttributes = new SerializableDictionary<string, string>();

                // create a string with the full qualified name of the SemSync connector
                var sourceConnectorName = typeof(ContactClient).FullName;

                // enumerate the properties of the contact that have no explicit mapping
                foreach (var x in Constants.PropertiesNotMapped)
                {
                    // get this properties 
                    var value = Tools.GetPropertyValueString(contact, x).Trim();

                    // the custom properties do have a "connected" boolean telling us if the value is specified 
                    // (the better alternative would have been to use nullable types ... but it's oracles choice, 
                    //  maybe nullables would have had more interoperability issues)
                    if (x.StartsWith("Custom", StringComparison.OrdinalIgnoreCase)
                        && !Tools.GetPropertyValueBoolean(contact, x + "Specified")
                        && !string.IsNullOrEmpty(Tools.GetPropertyValueString(contact, x + "Specified")))
                    {
                        // it's a custom attribute without being specified, so skip the rest
                        continue;
                    }

                    // only add non-null and non-empty values
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.SourceSpecificAttributes.Add(sourceConnectorName + "." + x, value);
                    }
                }
            }

            // clean up the values (set all defaults to NULL)
            SyncTools.ClearNulls(result, typeof(StdContact));

            var check = result.ToOracleContact(true);
            Tools.GetPropertyList(string.Empty, typeof(ContactData)).ForEach(
                x =>
                    {
                        if (Tools.GetPropertyValueString(check, x).Trim().Replace("\n", string.Empty) != Tools.GetPropertyValueString(contact, x).Trim().Replace("\n", string.Empty))
                    {
                        if (!x.EndsWith("Specified", StringComparison.OrdinalIgnoreCase) || Tools.GetPropertyValueBoolean(contact, x))
                        {
                            Tools.DebugWriteLine("problem");
                        }
                    }
                });

            return result;
        }

        /// <summary>
        /// Converts a <see cref="StdContact"/> to an Oracle CRM on Demand <see cref="ContactData"/>
        /// </summary>
        /// <param name="element">The standard contact</param>
        /// <param name="addCustomAttributes"> a value specifying whether non-mapped properties should be added from the <see cref="StdContact.SourceSpecificAttributes"/></param>
        /// <returns>The converted data as a <see cref="ContactData"/></returns>
        internal static ContactData ToOracleContact(this StdElement element, bool addCustomAttributes)
        {
            var contact = element as StdContact;
            if (contact == null)
            {
                return null;
            }

            var result = new ContactData
            {
                Id = contact.ExternalIdentifier[ProfileIdentifierType.OracleCrmOnDemandId],

                CreatedDate = contact.InternalSyncData.NewIfNull().DateOfCreation,
                CreatedDateSpecified = true,

                ModifiedDate = contact.InternalSyncData.NewIfNull().DateOfLastChange,
                ModifiedDateSpecified = true,

                Description = contact.AdditionalTextData,
                WorkPhone = contact.BusinessAddressPrimary.NewIfNull().Phone.NewIfNull().ToString().Replace("(", string.Empty).Replace(")", string.Empty),
                PrimaryCity = contact.BusinessAddressPrimary.NewIfNull().CityName,
                PrimaryZipCode = contact.BusinessAddressPrimary.NewIfNull().PostalCode,
                PrimaryCountry = contact.BusinessAddressPrimary.NewIfNull().CountryName,
                PrimaryAddress = contact.BusinessAddressPrimary.NewIfNull().StreetName,
                PrimaryProvince = contact.BusinessAddressPrimary.NewIfNull().StateName,

                HomePhone = contact.PersonalAddressPrimary.NewIfNull().Phone.NewIfNull().ToString().Replace("(", string.Empty).Replace(")", string.Empty),
                ContactEmail = contact.BusinessEmailPrimary,
                CellularPhone = contact.BusinessPhoneMobile.NewIfNull().ToString().Replace("(", string.Empty).Replace(")", string.Empty),
                JobTitle = contact.BusinessPosition,
                Department = contact.BusinessDepartment,
                AccountName = contact.BusinessCompanyName,

                DateofBirth = contact.DateOfBirth,
                DateofBirthSpecified = contact.DateOfBirth > DateTime.MinValue,
                
                ContactFirstName = contact.Name.NewIfNull().FirstName,
                MiddleName = contact.Name.NewIfNull().MiddleName,
                ContactLastName = contact.Name.NewIfNull().LastName,
            };

            if (addCustomAttributes && contact.SourceSpecificAttributes != null)
            {
                // determine the prefix for the key
                var targetConnectorName = typeof(ContactClient).FullName;
                if (targetConnectorName == null)
                {
                    return result;
                }

                var startIndex = targetConnectorName.Length;
                
                // get all matching source properties and set the target 
                // property + the corresponding "specified" property.
                (from x in contact.SourceSpecificAttributes
                 where x.Key.StartsWith(targetConnectorName, StringComparison.OrdinalIgnoreCase)
                 select x).ForEach(attribute =>
                     {
                         var key = attribute.Key.Substring(startIndex);
                         var keySpecified = key + "Specified";
                         Tools.SetPropertyValue(result, key, attribute.Value);
                         Tools.SetPropertyValue(result, keySpecified, "true");
                     });
            }

            return result;
        }
    }
}