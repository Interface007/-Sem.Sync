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
        public Func<string, FritzApi> FritzApiCreator { get; set; }

        public ContactClient()
        {
            this.FritzApiCreator =  clientFolderName => new FritzApi { Host = new Uri(clientFolderName), UserPassword = this.LogOnPassword, };
        }


        /// <summary>
        /// Overridden to not get the elements before writing them.
        /// </summary>
        /// <param name="elements">the elements to be written</param>
        /// <param name="clientFolderName">the URI for the FritzBox address</param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Writes all entries to the FritzBox
        /// </summary>
        /// <param name="elements">the elements to be written</param>
        /// <param name="clientFolderName">the URI for the FritzBox address</param>
        /// <param name="skipIfExisting"> Ignored in this implementation. </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            // initialize API
            var fritzApi = FritzApiCreator.Invoke(clientFolderName); 

            // create a book with all entries that have a business or a private phone number
            var book = new PhoneBook(
                            from x in elements.ToStdContacts()
                            where (x.PersonalAddressPrimary != null && x.PersonalAddressPrimary.Phone != null)
                               || (x.BusinessAddressPrimary != null && x.BusinessAddressPrimary.Phone != null)
                            select new Contact
                                {
                                    Person = new Person(x.Name.ToString())
                                        {
                                            ImageUrl = x.SourceSpecificAttributes.NewIfNull().GetValue("FritzBox.ImageUrl")
                                        },
                                    Telephony = new List<Entities.PhoneNumber>
                                                {
                                                    new Entities.PhoneNumber(PhoneNumberType.Home, x.PersonalAddressPrimary.NewIfNull().Phone.NewIfNull().ToString()),
                                                    new Entities.PhoneNumber(PhoneNumberType.Work, x.BusinessAddressPrimary.NewIfNull().Phone.NewIfNull().ToString()),
                                                },
                                    Category = (PersonCategory)Enum.Parse(typeof(PersonCategory), x.SourceSpecificAttributes.NewIfNull().GetValue("FritzBox.Category") ?? "Default"),
                                });

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

            var fritzApi = FritzApiCreator.Invoke(clientFolderName);

            var book = fritzApi.GetPhoneBook();
            result = (from entry in book
                      select
                          new StdContact
                              {
                                  Name = new PersonName(entry.Person.RealName),
                                  PersonalAddressPrimary = new AddressDetail { Phone = entry.Telephony.ExtractPhoneNumber(PhoneNumberType.Home) },
                                  BusinessAddressPrimary = new AddressDetail { Phone = entry.Telephony.ExtractPhoneNumber(PhoneNumberType.Work) },
                                  PersonalPhoneMobile = entry.Telephony.ExtractPhoneNumber(PhoneNumberType.Mobile),
                                  SourceSpecificAttributes =
                                      new SerializableDictionary<string, string>(
                                          new[]
                                              {
                                                  new KeyValuePair<string, string>("FritzBox.Category", entry.Category.ToString()),
                                                  new KeyValuePair<string, string>("FritzBox.ImageUrl", entry.Person.ImageUrl),
                                              }),
                              }).ToStdElements();

            result.Sort();
            this.Normalize(result);

            return result;
        }
    }
}
