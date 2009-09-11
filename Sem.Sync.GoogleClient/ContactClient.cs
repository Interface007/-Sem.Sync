﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.GoogleClient
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using GenericHelpers;

    using Google.Contacts;
    using Google.GData.Client;
    using Google.GData.Contacts;
    using Google.GData.Extensions;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    using PhoneNumber = SyncBase.DetailData.PhoneNumber;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(
        DisplayName = "Google Mail Contacts Client",
        CanRead = true,
        CanWrite = true,
        MatchingIdentifier = ProfileIdentifierType.Google,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// The schema prefix for the google data api
        /// </summary>
        private const string GoogleSchemaPrefix2005 = "http://schemas.google.com/g/2005#";

        /// <summary>
        /// The qualifier for the attribute "work"
        /// </summary>
        private const string GoogleSchemaQualifierWork = "work";

        /// <summary>
        /// The qualifier for the attribute "home"
        /// </summary>
        private const string GoogleSchemaQualifierHome = "home";

        /// <summary>
        /// The settings instance
        /// </summary>
        private RequestSettings settings;
        
        /// <summary>
        /// The requester object for the google api
        /// </summary>
        private ContactsRequest requester;

        /// <summary>
        /// An URI representing the currently logged in user
        /// </summary>
        private Uri contactsUri;

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Google-Contact-Client";
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            try
            {
                this.EnsureInitialization();

                var f = this.requester.GetContacts();
                foreach (var googleContact in f.Entries)
                {
                    try
                    {
                        var semSyncIdString = (from x in googleContact.ExtendedProperties where x.Name == "SemSyncId" select x.Value).FirstOrDefault();
                        var semSyncId = string.IsNullOrEmpty(semSyncIdString) ? Guid.NewGuid() : new Guid(semSyncIdString);

                        var stdEntry = new StdContact
                        {
                            Id = semSyncId,
                            Name = new PersonName(googleContact.Title),
                            PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.Google, googleContact.Id)
                        };

                        foreach (var address in googleContact.PostalAddresses)
                        {
                            var stdAddress = new AddressDetail(address.Value);
                            if (address.Home)
                            {
                                if (stdEntry.PersonalAddressPrimary == null)
                                {
                                    stdEntry.PersonalAddressPrimary = stdAddress;
                                }
                                else
                                {
                                    stdEntry.PersonalAddressSecondary = stdAddress;
                                }
                            }

                            if (address.Work)
                            {
                                if (stdEntry.BusinessAddressPrimary == null)
                                {
                                    stdEntry.BusinessAddressPrimary = stdAddress;
                                }
                                else
                                {
                                    stdEntry.BusinessAddressSecondary = stdAddress;
                                }
                            }
                        }

                        foreach (var organization in googleContact.Organizations)
                        {
                            if (organization.Primary)
                            {
                                stdEntry.BusinessPosition = organization.Title;
                                stdEntry.BusinessCompanyName = organization.Name;
                                break;
                            }
                        }

                        foreach (var phonenumber in googleContact.Phonenumbers)
                        {
                            var stdPhoneNumber = new PhoneNumber(phonenumber.Value);
                            if (phonenumber.Home)
                            {
                                stdEntry.PersonalAddressPrimary = stdEntry.PersonalAddressPrimary ?? new AddressDetail();
                                stdEntry.PersonalAddressPrimary.Phone = stdPhoneNumber;
                            }

                            if (phonenumber.Work)
                            {
                                stdEntry.BusinessAddressPrimary = stdEntry.BusinessAddressPrimary ?? new AddressDetail();
                                stdEntry.BusinessAddressPrimary.Phone = stdPhoneNumber;
                            }
                        }

                        foreach (var email in googleContact.Emails)
                        {
                            if (email.Home)
                            {
                                stdEntry.PersonalEmailPrimary = email.Value;
                            }

                            if (email.Work)
                            {
                                stdEntry.BusinessEmailPrimary = email.Value;
                            }
                        }

                        if (googleContact.PhotoUri != null)
                        {
                            try
                            {
                                using (var stream = this.requester.GetPhoto(googleContact))
                                {
                                    if (stream != null)
                                    {
                                        var value = new StreamReader(stream).ReadToEnd();
                                        stdEntry.PictureData = Encoding.ASCII.GetBytes(value);
                                    }
                                }
                            }
                            catch (GDataNotModifiedException ex)
                            {
                                var helper = HttpHelper.DefaultInstance;
                                helper.ContentCredentials.LogOnDomain = "[GOOGLE]";
                                helper.ContentCredentials.LogOnPassword = ((GDataGAuthRequestFactory)this.requester.Service.RequestFactory).GAuthToken;
                                stdEntry.PictureData = helper.GetContentBinary(googleContact.PhotoUri.AbsoluteUri, string.Empty, string.Empty);
                                this.LogProcessingEvent("Error while executing client: {0}", ex.Message);
                            }
                        }

                        result.Add(stdEntry);
                    }
                    catch (GDataRequestException ex)
                    {
                        this.LogProcessingEvent("Error while executing client: {0}", ex.Message);
                    }
                }
            }
            catch (GDataRequestException ex)
            {
                this.LogProcessingEvent("Error while executing client: {0}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full name including path of the file that will get the contacts while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.EnsureInitialization();

            foreach (var stdContact in elements.ToContacts())
            {
                try
                {
                    var googleId = stdContact.PersonalProfileIdentifiers.GoogleId;

                    var googleContact =
                        (!string.IsNullOrEmpty(googleId)
                             ? this.requester.Retrieve<Contact>(new Uri(googleId))
                             : null) ?? new Contact();

                    ExtendedProperty semSyncId = null;
                    if (googleContact.ExtendedProperties != null)
                    {
                        if (googleContact.ExtendedProperties.Count > 0)
                        {
                            semSyncId = (from x in googleContact.ExtendedProperties where x.Name == "SemSyncId" select x).FirstOrDefault();
                        }
                        
                        if (semSyncId == null)
                        {
                            semSyncId = new ExtendedProperty { Name = stdContact.Id.ToString(), Value = "SemSyncId" };
                            googleContact.ExtendedProperties.Add(semSyncId);
                        }
                    }

                    googleContact.Title = stdContact.Name.ToString();

                    if (!string.IsNullOrEmpty(stdContact.PersonalEmailPrimary))
                    {
                        googleContact.Emails.Add(new EMail(stdContact.PersonalEmailPrimary, GoogleSchemaPrefix2005 + GoogleSchemaQualifierHome));
                    }

                    if (!string.IsNullOrEmpty(stdContact.BusinessEmailPrimary))
                    {
                        googleContact.Emails.Add(new EMail(stdContact.BusinessEmailPrimary, GoogleSchemaPrefix2005 + GoogleSchemaQualifierWork));
                    }

                    AddAddressToGoogleContact(googleContact, stdContact.PersonalAddressPrimary, GoogleSchemaQualifierHome);
                    AddAddressToGoogleContact(googleContact, stdContact.PersonalAddressSecondary, GoogleSchemaQualifierHome);
                    AddAddressToGoogleContact(googleContact, stdContact.BusinessAddressPrimary, GoogleSchemaQualifierWork);
                    AddAddressToGoogleContact(googleContact, stdContact.BusinessAddressSecondary, GoogleSchemaQualifierWork);

                    if (string.IsNullOrEmpty(googleId))
                    {
                        // replace the google contact with the updated version
                        googleContact = this.requester.Insert(this.contactsUri, googleContact);
                        stdContact.PersonalProfileIdentifiers.GoogleId = googleContact.Id;
                    }
                    else
                    {
                        googleContact = this.requester.Update(googleContact);
                    }

                    this.UpdateGooglePhoto(stdContact, googleContact);
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Adds a specific <see cref="AddressDetail"/> to the google address list of a google contact
        /// </summary>
        /// <param name="googleContact"> The google contact. </param>
        /// <param name="stdAddress"> The <see cref="AddressDetail"/> data from the <see cref="StdContact"/>. </param>
        /// <param name="addressType"> A text type of address ("home", "work"). </param>
        private static void AddAddressToGoogleContact(Contact googleContact, AddressDetail stdAddress, string addressType)
        {
            if (stdAddress != null)
            {
                var addressText = stdAddress.ToString(AddressFormatting.StreetAndCity);
                if (!string.IsNullOrEmpty(addressText))
                {
                    var postalAddress = new PostalAddress(addressText)
                    {
                        Rel = GoogleSchemaPrefix2005 + addressType,
                    };

                    if (!IsAddressExisting(googleContact.PostalAddresses, stdAddress))
                    {
                        googleContact.PostalAddresses.Add(postalAddress);
                    }
                }

                if (stdAddress.Phone != null && !string.IsNullOrEmpty(stdAddress.Phone.ToString()))
                {
                    var phone = new Google.GData.Extensions.PhoneNumber(stdAddress.Phone.ToString())
                        {
                            Rel = GoogleSchemaPrefix2005 + addressType
                        };

                    googleContact.Phonenumbers.Add(phone);
                }
            }
        }

        /// <summary>
        /// Searches for a matching address inside the google address collection.
        /// </summary>
        /// <param name="googleAddresses"> The google address collection. </param>
        /// <param name="stdAddress"> The <see cref="AddressDetail"/> data from the <see cref="StdContact"/>. </param>
        /// <returns> true if the address is already part of the collection </returns>
        private static bool IsAddressExisting(IEnumerable<PostalAddress> googleAddresses, AddressDetail stdAddress)
        {
            if (stdAddress != null)
            {
                foreach (var address in googleAddresses)
                {
                    if (address.Value == stdAddress.ToString(AddressFormatting.StreetAndCity))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// logs the exception to the console and the <see cref="SyncComponent.LogProcessingEvent(object,Sem.GenericHelpers.EventArgs.ProcessingEventArgs)"/>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        private void LogError(Exception exception)
        {
            Console.WriteLine(exception + " : " + exception.Message);
            this.LogProcessingEvent("Error while executing client: {0}", exception.Message);
        }
        
        /// <summary>
        /// Creates or updates photo information for a google contact with the infoprmation found in the
        /// std-contact.
        /// </summary>
        /// <param name="contact">The contact containing the image information.</param>
        /// <param name="googleContact"> The google contact. </param>
        private void UpdateGooglePhoto(StdContact contact, Contact googleContact)
        {
            if (contact.PictureData == null || contact.PictureData.Length <= 0 || string.IsNullOrEmpty(googleContact.Id))
            {
                return;
            }

            var rs = new RequestSettings("Sem.Sync.GoogleClient", this.LogOnUserId, this.LogOnPassword) { AutoPaging = true };
            var cr = new ContactsRequest(rs);

            using (var photoStream = new MemoryStream(contact.PictureData))
            {
                try
                {
                    cr.SetPhoto(googleContact, photoStream);
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Initializes the object after calling the <see cref="StdClient.ReadFullList"/> or <see cref="StdClient.WriteFullList"/> method.
        /// This performs a setup of the requester to query the Google Data API.
        /// </summary>
        private void EnsureInitialization()
        {
            if (this.settings == null)
            {
                var userName = this.LogOnUserId;
                var passWord = this.LogOnPassword;

                this.settings = new RequestSettings("Sem.Sync.GoogleClient", userName, passWord)
                    {
                        AutoPaging = true
                    };
            }

            this.requester = this.requester ?? new ContactsRequest(this.settings);

            if (this.contactsUri == null)
            {
                this.contactsUri = new Uri(ContactsQuery.CreateContactsUri(this.LogOnUserId, GroupsQuery.fullProjection));
            }
        }
    }
}
