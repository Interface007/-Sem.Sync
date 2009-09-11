// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleContactMappingExtensions.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Mapping class that contains only mapping logic between google contacts and sem.sync contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.GoogleClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using GenericHelpers;
    using GenericHelpers.Interfaces;

    using Google.Contacts;
    using Google.GData.Client;
    using Google.GData.Extensions;

    using SyncBase;
    using SyncBase.DetailData;

    using PhoneNumber = SyncBase.DetailData.PhoneNumber;

    /// <summary>
    /// Mapping class that contains only mapping logic between google contacts and sem.sync contacts
    /// </summary>
    public static class GoogleContactMappingExtensions
    {
        /// <summary>
        /// The schema prefix for the google data api
        /// </summary>
        private const string GoogleSchemaPrefix2005 = "http://schemas.google.com/g/2005#";

        /// <summary>
        /// Adds a specific <see cref="AddressDetail"/> to the google address list of a google contact
        /// </summary>
        /// <param name="googleContact"> The google contact. </param>
        /// <param name="stdAddress"> The <see cref="AddressDetail"/> data from the <see cref="StdContact"/>. </param>
        /// <param name="addressType"> A text type of address ("home", "work"). </param>
        public static void AddAddress(this Contact googleContact, AddressDetail stdAddress, string addressType)
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
        /// Creates or updates photo information for a google contact with the infoprmation found in the
        /// std-contact.
        /// </summary>
        /// <param name="googleContact"> The google contact.  </param>
        /// <param name="contact"> The contact containing the image information. </param>
        /// <param name="credentials"> The credentials providing object. </param>
        public static void UpdatePhoto(this Contact googleContact, StdContact contact, ICredentialAware credentials)
        {
            if (contact.PictureData == null || contact.PictureData.Length <= 0 || string.IsNullOrEmpty(googleContact.Id))
            {
                return;
            }

            var rs = new RequestSettings("Sem.Sync.GoogleClient", credentials.LogOnUserId, credentials.LogOnPassword) { AutoPaging = true };
            var cr = new ContactsRequest(rs);

            using (var photoStream = new MemoryStream(contact.PictureData))
            {
                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        cr.SetPhoto(googleContact, photoStream);
                        break;
                    }
                    catch (GDataRequestException ex)
                    {
                        ((ContactClient)credentials).LogError(ex);
                        if (ex.ResponseString != string.Empty)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ((ContactClient)credentials).LogError(ex);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// adds email information to a google contact
        /// </summary>
        /// <param name="googleContact"> The google contact to add the information to. </param>
        /// <param name="email"> The email address. </param>
        /// <param name="addressType"> The address type (home or work). </param>
        public static void AddEmail(this Contact googleContact, string email, string addressType)
        {
            if (!string.IsNullOrEmpty(email))
            {
                googleContact.Emails.Add(new EMail(email, GoogleSchemaPrefix2005 + addressType));
            }
        }

        /// <summary>
        /// Sets the sem.sync guid into the contact
        /// </summary>
        /// <param name="googleContact"> The google contact to add the information to. </param>
        /// <param name="identifier"> The identifier. </param>
        public static void SetSyncIdentifier(this Contact googleContact, Guid identifier)
        {
            ExtendedProperty semSyncId = null;
            if (googleContact.ExtendedProperties != null)
            {
                if (googleContact.ExtendedProperties.Count > 0)
                {
                    semSyncId = (from x in googleContact.ExtendedProperties where x.Name == "SemSyncId" select x).FirstOrDefault();
                }

                if (semSyncId == null)
                {
                    semSyncId = new ExtendedProperty { Name = identifier.ToString(), Value = "SemSyncId" };
                    googleContact.ExtendedProperties.Add(semSyncId);
                }
            }
        }

        /// <summary>
        /// Sets the organization info including the business position.
        /// </summary>
        /// <param name="stdEntry"> The std entry. </param>
        /// <param name="organization"> The organization information from the google contact. </param>
        public static void SetBusiness(this StdContact stdEntry, Organization organization)
        {
            if (organization.Primary)
            {
                stdEntry.BusinessPosition = organization.Title;
                stdEntry.BusinessCompanyName = organization.Name;
            }
        }

        /// <summary>
        /// doenloads the image for a contact
        /// </summary>
        /// <param name="stdEntry"> The std entry. </param>
        /// <param name="googleContact"> The google contact. </param>
        /// <param name="requester"> The requester. </param>
        public static void SetPicture(this StdContact stdEntry, Contact googleContact, ContactsRequest requester)
        {
            if (googleContact == null || googleContact.PhotoUri == null)
            {
                return;
            }

            try
            {
                using (var stream = requester.GetPhoto(googleContact))
                {
                    if (stream != null)
                    {
                        var value = new StreamReader(stream).ReadToEnd();
                        stdEntry.PictureData = Encoding.ASCII.GetBytes(value);
                    }
                }
            }
            catch (GDataNotModifiedException)
            {
                var helper = HttpHelper.DefaultInstance;
                helper.ContentCredentials.LogOnDomain = "[GOOGLE]";
                helper.ContentCredentials.LogOnPassword = ((GDataGAuthRequestFactory)requester.Service.RequestFactory).GAuthToken;
                stdEntry.PictureData = helper.GetContentBinary(googleContact.PhotoUri.AbsoluteUri, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// sets the correct email address from a google contacts email
        /// </summary>
        /// <param name="stdEntry"> The std entry. </param>
        /// <param name="email"> The email address. </param>
        public static void SetEmail(this StdContact stdEntry, EMail email)
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

        /// <summary>
        /// sets the correct phone number from a google contacts email
        /// </summary>
        /// <param name="stdEntry"> The std entry. </param>
        /// <param name="phonenumber"> The phone number to set into the std entry. </param>
        public static void SetPhone(this StdContact stdEntry, Google.GData.Extensions.PhoneNumber phonenumber)
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

        /// <summary>
        /// Sets a postal address from the google contact into the std contact.
        /// </summary>
        /// <param name="stdEntry"> The std entry. </param>
        /// <param name="address"> The address. </param>
        public static void SetAddress(this StdContact stdEntry, PostalAddress address)
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
    }
}
