// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Sem.Sync.Connector.ExchangeWebServiceManagedApi
{
    using System;
    
    using Microsoft.Exchange.WebServices.Data;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Public extension methods for conversion of data.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// transforms an Exchange phone entry (<see cref="PhoneNumberEntry"/>) into a <see cref="PhoneNumber"/>.
        /// </summary>
        /// <param name="element"> The exchange phone number. </param>
        /// <returns> the sem.sync phone number </returns>
        public static PhoneNumber ToPhoneNumber(this PhoneNumberEntry element)
        {
            if (element == null)
            {
                return null;
            }

            return new PhoneNumber(element.PhoneNumber);
        }

        /// <summary>
        /// Converts an Exchange <see cref="PhysicalAddressEntry"/> into a Sem.Sync <see cref="AddressDetail"/>.
        /// </summary>
        /// <param name="element"> The exchange address entry. </param>
        /// <param name="phoneNumber"> The corresponding phone number. </param>
        /// <returns> The <see cref="AddressDetail"/> including the <see cref="PhoneNumber"/> </returns>
        public static AddressDetail ToAddressDetail(this PhysicalAddressEntry element, string phoneNumber)
        {
            if (element == null)
            {
                return null;
            }

            return new AddressDetail
                       {
                           CityName = element.City,
                           CountryName = element.CountryOrRegion,
                           PostalCode = element.PostalCode,
                           StreetName = element.Street,
                           StateName = element.State,
                           Phone = phoneNumber
                       };
        }

        /// <summary>
        /// Converts an exchange item to a <see cref="StdContact"/> if possible - it will return NULL if this conversion is not possible.
        /// </summary>
        /// <param name="element"> The element to be converted. </param>
        /// <returns> the <see cref="StdContact"/> that did result from this conversion </returns>
        public static StdContact ToStdContact(this Item element)
        {
            var contact = element as Contact;
            if (contact == null)
            {
                return null;
            }

            if (contact.ImAddresses.Contains(ImAddressKey.ImAddress1))
            {
                var imAddress = contact.ImAddresses[ImAddressKey.ImAddress1];
                if (imAddress != null)
                {
                    Console.WriteLine(imAddress);
                }
            }

            var result = new StdContact();
            result.InternalSyncData = new SyncData
                                        {
                                            DateOfCreation = element.DateTimeCreated,
                                            DateOfLastChange = element.LastModifiedTime
                                        };

            result.PersonalProfileIdentifiers = new ProfileIdentifiers
                                        {
                                            ExchangeWs = contact.Id.UniqueId
                                        };
            
            result.Name = new PersonName
                              {
                                  LastName = contact.Surname,
                                  FirstName = contact.GivenName,
                                  MiddleName = contact.MiddleName,
                                  AcademicTitle = contact.CompleteName.Title,
                                  Suffix = contact.CompleteName.Suffix
                              };

            EatException(
                () => result.DateOfBirth = contact.Birthday ?? new DateTime(), 
                (ServiceObjectPropertyException ex) => true);

            result.AdditionalTextData = contact.Body;
            result.Categories = contact.Categories == null ? null : new List<string>(contact.Categories);

            result.BusinessCompanyName = contact.CompanyName;
            result.BusinessDepartment = contact.Department;
            result.BusinessPosition = contact.JobTitle;
            result.BusinessHomepage = contact.BusinessHomePage;
            
            result.BusinessAddressPrimary = GetAddress(contact, PhysicalAddressKey.Business).ToAddressDetail(GetPhone(contact, PhoneNumberKey.BusinessPhone));
            result.PersonalAddressPrimary = GetAddress(contact, PhysicalAddressKey.Home).ToAddressDetail(GetPhone(contact, PhoneNumberKey.HomePhone));
            result.PersonalAddressSecondary = GetAddress(contact, PhysicalAddressKey.Other).ToAddressDetail(GetPhone(contact, PhoneNumberKey.HomePhone2));

            result.BusinessPhoneMobile = new PhoneNumber(GetPhone(contact, PhoneNumberKey.MobilePhone));
            result.PersonalPhoneMobile = new PhoneNumber(GetPhone(contact, PhoneNumberKey.OtherTelephone));
            
            result.BusinessEmailPrimary = contact.GetEMail(EmailAddressKey.EmailAddress1);
            result.PersonalEmailPrimary = contact.GetEMail(EmailAddressKey.EmailAddress2);
            result.PersonalEmailSecondary = contact.GetEMail(EmailAddressKey.EmailAddress3);

            return result;
        }

        /// <summary>
        /// Converts a <see cref="StdContact" /> to an Exchange API <see cref="Contact"/>
        /// </summary>
        /// <param name="contact"> The contact to be converted. </param>
        /// <param name="service"> The exchange service. </param>
        /// <returns> the converted contact element </returns>
        public static Contact ToExchangeContact(this StdContact contact, ExchangeService service)
        {
            var exchangeContact = new Contact(service);

            exchangeContact.GivenName = contact.Name.FirstName;
            exchangeContact.MiddleName = contact.Name.MiddleName;
            exchangeContact.Surname = contact.Name.LastName;

            exchangeContact.Birthday = contact.DateOfBirth;
            exchangeContact.Body = contact.AdditionalTextData;
            exchangeContact.Categories = contact.Categories == null ? null : new StringList(contact.Categories);
            
            exchangeContact.CompanyName = contact.BusinessCompanyName;
            exchangeContact.Department = contact.BusinessDepartment;
            exchangeContact.JobTitle = contact.BusinessPosition;
            exchangeContact.BusinessHomePage = contact.BusinessHomepage;

            exchangeContact.SetAddress(PhysicalAddressKey.Business, contact.BusinessAddressPrimary);
            exchangeContact.SetAddress(PhysicalAddressKey.Home, contact.PersonalAddressPrimary);
            exchangeContact.SetAddress(PhysicalAddressKey.Other, contact.PersonalAddressSecondary);

            exchangeContact.SetPhoneNumber(PhoneNumberKey.HomePhone, contact.PersonalAddressPrimary.NewIfNull().Phone.NewIfNull().ToString());
            exchangeContact.SetPhoneNumber(PhoneNumberKey.HomePhone2, contact.PersonalAddressSecondary.NewIfNull().Phone.NewIfNull().ToString());
            exchangeContact.SetPhoneNumber(PhoneNumberKey.BusinessPhone, contact.BusinessAddressPrimary.NewIfNull().Phone.NewIfNull().ToString());
            exchangeContact.SetPhoneNumber(PhoneNumberKey.BusinessPhone2, contact.BusinessAddressSecondary.NewIfNull().Phone.NewIfNull().ToString());

            exchangeContact.SetPhoneNumber(PhoneNumberKey.MobilePhone, contact.BusinessPhoneMobile.NewIfNull().ToString());
            exchangeContact.SetPhoneNumber(PhoneNumberKey.OtherTelephone, contact.PersonalPhoneMobile.NewIfNull().ToString());

            exchangeContact.SetImAddress(ImAddressKey.ImAddress1, contact.BusinessInstantMessengerAddresses.NewIfNull().MsnMessenger);
            exchangeContact.SetImAddress(ImAddressKey.ImAddress2, contact.PersonalInstantMessengerAddresses.NewIfNull().MsnMessenger);
            exchangeContact.SetImAddress(ImAddressKey.ImAddress3, contact.PersonalInstantMessengerAddresses.NewIfNull().GoogleTalk);

            exchangeContact.SetEmailAddress(EmailAddressKey.EmailAddress1, contact.BusinessEmailPrimary);
            exchangeContact.SetEmailAddress(EmailAddressKey.EmailAddress2, contact.PersonalEmailPrimary);
            exchangeContact.SetEmailAddress(EmailAddressKey.EmailAddress3, contact.PersonalEmailSecondary);

            return exchangeContact;
        }

        /// <summary>
        /// Simply suppresses <see cref="NullReferenceException"/> and <see cref="ServiceObjectPropertyException"/> in order to not 
        /// write this code again and again.
        /// </summary>
        /// <typeparam name="T"> the exception to eat </typeparam>
        /// <param name="code"> The code to be executed.   </param>
        /// <param name="check"> The check function - when this expression is true, the exception is not thrown.  </param>
        public static void EatException<T>(Action code, Func<T, bool> check) where T : Exception
        {
            try
            {
                code.Invoke();
            }
            catch (T ex)
            {
                if (!check(ex))
                {
                    throw;
                }
            }
        }

        private static void SetAddress(this Contact exchangeContact, PhysicalAddressKey address, AddressDetail value)
        {
            if (value == null)
            {
                return;
            }

            var exchangeAddress = new PhysicalAddressEntry
                                      {
                                          City = value.CityName,
                                          CountryOrRegion = value.CountryName,
                                          PostalCode = value.PostalCode,
                                          State = value.StateName,
                                          Street = value.StreetName
                                      };

            exchangeContact.PhysicalAddresses[address] = exchangeAddress;
        }

        /// <summary>
        /// sets a phone number if it's not null
        /// </summary>
        /// <param name="exchangeContact"> The exchange contact. </param>
        /// <param name="address"> The phone number type to be set. </param>
        /// <param name="value"> The value (the phone number). </param>
        private static void SetPhoneNumber(this Contact exchangeContact, PhoneNumberKey address, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                exchangeContact.PhoneNumbers[address] = value;
            }
        }

        /// <summary>
        /// sets a instant messenger address if it's not null
        /// </summary>
        /// <param name="exchangeContact"> The exchange contact. </param>
        /// <param name="address"> The im address type to be set. </param>
        /// <param name="value"> The value (the im address). </param>
        private static void SetImAddress(this Contact exchangeContact, ImAddressKey address, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                exchangeContact.ImAddresses[address] = value;
            }
        }

        /// <summary>
        /// sets an email if it's not null
        /// </summary>
        /// <param name="exchangeContact"> The exchange contact. </param>
        /// <param name="address"> The email type to be set. </param>
        /// <param name="value"> The value (the email address). </param>
        private static void SetEmailAddress(this Contact exchangeContact, EmailAddressKey address, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                exchangeContact.EmailAddresses[address] = value;
            }
        }

        /// <summary>
        /// Gets an address entry if it's defined - null if the address is not defined in this contact.
        /// </summary>
        /// <param name="contact"> The contact potentially containing the address. </param>
        /// <param name="addressKey"> The address Key. </param>
        /// <returns> The address entry.  </returns>
        private static PhysicalAddressEntry GetAddress(this Contact contact, PhysicalAddressKey addressKey)
        {
            return contact.PhysicalAddresses.Contains(addressKey)
                       ? contact.PhysicalAddresses[addressKey]
                       : null;
        }

        /// <summary>
        /// Gets an email address entry if it's defined - null if the email address is not defined in this contact.
        /// </summary>
        /// <param name="contact"> The contact potentially containing the email address. </param>
        /// <param name="emailKey"> The email address Key. </param>
        /// <returns> The email address entry.  </returns>
        private static string GetEMail(this Contact contact, EmailAddressKey emailKey)
        {
            return contact.EmailAddresses.Contains(emailKey)
                       ? contact.EmailAddresses[emailKey].Address
                       : string.Empty;
        }

        /// <summary>
        /// Gets a phone number entry if it's defined - null if the number is not defined in this contact.
        /// </summary>
        /// <param name="contact"> The contact potentially containing the phone number. </param>
        /// <param name="phoneKey"> The phone number Key. </param>
        /// <returns> The phone number entry.  </returns>
        private static string GetPhone(this Contact contact, PhoneNumberKey phoneKey)
        {
            return contact.PhoneNumbers.Contains(phoneKey)
                       ? contact.PhoneNumbers[phoneKey]
                       : null;
        }
    }
}
