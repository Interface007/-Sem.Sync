// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.FritzBox.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    using PhoneNumber = Sem.Sync.SyncBase.DetailData.PhoneNumber;

    /// <summary>
    /// This class is the client class for handling elements. This class leaks some of the contact 
    ///   related features of "ContactClient", but provides the ability to handle other types that do 
    ///   inherit from StdElement
    /// </summary>
    [ClientStoragePathDescription(
        Mandatory = true,
        Default = @"http://fritz.box/",
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Fritz!Box AddressBook Client",
        NeedsCredentials = true,
        NeedsCredentialsDomain = true)]
    public class ContactClient : StdClient
    {
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            var fritzApi = new FritzApi
            {
                Host = new Uri("http://192.168.1.9/"),
                UserPassword = string.Empty
            };

            var book = new PhoneBook();

            (from x in elements.ToStdContacts()
             where (x.PersonalAddressPrimary != null && x.PersonalAddressPrimary.Phone != null)
                || (x.BusinessAddressPrimary != null && x.BusinessAddressPrimary.Phone != null)
             select new Contact
                 {
                     Person = new Person
                         {
                             RealName = x.Name.ToString()
                         },
                     Telephony = new List<Entities.PhoneNumber>()
                                {
                                    new Entities.PhoneNumber()
                                        {
                                            Number = x.PersonalAddressPrimary.NewIfNull().Phone.NewIfNull().ToString(),
                                            DestinationType = PhoneNumberType.Home
                                        },
                                    new Entities.PhoneNumber()
                                        {
                                            Number = x.BusinessAddressPrimary.NewIfNull().Phone.NewIfNull().ToString(),
                                            DestinationType = PhoneNumberType.Work
                                        }
                                }
                 }).ForEach(book.Add);

            fritzApi.ClearPhoneBook();
            fritzApi.SetPhoneBook(book);

            this.LogProcessingEvent("{0} entries added", book.Count);
        }

        /// <summary>
        /// Reads the contacts from the FritzBox
        /// </summary>
        /// <param name="clientFolderName"> The url to the fritz box. </param>
        /// <param name="result"> The empty prepared result list. </param>
        /// <returns> The result list of the read operation </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            var phoneBook = new FritzApi
                {
                    Host = new Uri(clientFolderName),
                    UserPassword = this.LogOnPassword
                };

            result = (from entry in phoneBook.GetPhoneBook()
                      select new StdContact
                          {
                              Name = new PersonName(entry.Person.RealName),
                              PersonalAddressPrimary = new AddressDetail
                                   {
                                       Phone = new PhoneNumber((
                                           from x in entry.Telephony
                                           where x.DestinationType == PhoneNumberType.Home
                                           orderby x.Priority
                                           select x.Number).FirstOrDefault())
                                   },
                              BusinessAddressPrimary = new AddressDetail
                                   {
                                       Phone = new PhoneNumber((
                                           from x in entry.Telephony
                                           where x.DestinationType == PhoneNumberType.Work
                                           orderby x.Priority
                                           select x.Number).FirstOrDefault())
                                   },
                              PersonalPhoneMobile = new PhoneNumber((
                                           from x in entry.Telephony
                                           where x.DestinationType == PhoneNumberType.Mobile
                                           orderby x.Priority
                                           select x.Number).FirstOrDefault())
                          }).ToStdElements();

            result.Sort();
            this.Normalize(result);

            return result;
        }
    }
}
