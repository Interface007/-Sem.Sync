// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    using System;
    using System.Collections.Generic;

    using global::Facebook.Components;
    using global::Facebook.Entity;

    using Microsoft.Win32;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    using Gender = SyncBase.DetailData.Gender;

    /// <summary>
    /// Implements a reading client for the Facebook api. Unfortunately Facebook is not
    /// publishing too much data about friends.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(CanRead = true, CanWrite = false, NeedsCredentials = true,
        DisplayName = "Facebook", MatchingIdentifier = ProfileIdentifierType.FacebookProfileId)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// Facebook does need the key to allow the access. This key does identify the application.
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// Facebook does need the secret as a password to allow the access.
        /// Unfortunately Facebook does not allow sharing the secret.
        /// </summary>
        private readonly string apiSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class and initializes the apiKey 
        /// and the apiSecret from the registry. Unfortunately this also means that we need every user
        /// to request such a key and secret in order to work with the client.
        /// </summary>
        public ContactClient()
        {
            // Read the application key and secret from the registry. This way it cannot leak by
            // publishing the app. Todo: we need to use the "temporary secret" as suggested by Facebook
            // the problem with the temporary secret is that it does not really add security and it
            // includes another web server that is hosted by the application owner.
            var regKey = Registry.CurrentUser.OpenSubKey("Software\\" + this.FriendlyClientName)
                    ?? Registry.CurrentUser.CreateSubKey("Software\\" + this.FriendlyClientName);

            if (regKey != null)
            {
                var regValue = regKey.GetValue("apiKey");
                if (regValue == null)
                {
                    regKey.SetValue("apiKey", "enter your API-Key here");
                }
                else
                {
                    this.apiKey = regValue.ToString();
                }

                regValue = regKey.GetValue("apiSecret");
                if (regValue == null)
                {
                    regKey.SetValue("apiSecret", "enter your API-Secret here");
                }
                else
                {
                    this.apiSecret = regValue.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override sealed string FriendlyClientName
        {
            get { return "Facebook-Connector"; }
        }

        /// <summary>
        /// Read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">Facebook does not need this parameter.</param>
        /// <param name="result">The list of elements that should get the elements. This parameter is not used 
        /// by this parameter - a new list is created.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var resultList = new List<StdElement>();
            var service = new FacebookService { ApplicationKey = this.apiKey, Secret = this.apiSecret };

            service.ConnectToFacebook();
            var friendList = service.GetFriendIds();
            foreach (var friend in friendList)
            {
                User userData = null;
                try
                {
                    userData = service.GetUserInfo(friend)[0];
                    LogProcessingEvent("converting " + userData.LastName + ", " + userData.Name);
                }
                catch (Exception ex)
                {
                    LogProcessingEvent("Exception: " + ex.Message);
                }

                if (userData != null)
                {
                    var pictureBytes =
                        userData.PictureBigBytes ??
                        userData.PictureBytes ??
                        userData.PictureSmallBytes ??
                        userData.PictureSquareBytes;

                    resultList.Add(
                        new StdContact
                            {
                                Id = Guid.NewGuid(),

                                InternalSyncData = new SyncData { DateOfLastChange = userData.ProfileUpdateDate },

                                PersonGender =
                                    (userData.Sex == global::Facebook.Entity.Gender.Female)
                                        ? Gender.Female
                                        :
                                            (userData.Sex == global::Facebook.Entity.Gender.Male)
                                                ? Gender.Male
                                                :
                                                    Gender.Unspecified,

                                Name = new PersonName
                                           {
                                               LastName = userData.LastName,
                                               FirstName = userData.FirstName,
                                           },

                                DateOfBirth = userData.Birthday ?? new DateTime(),

                                PersonalAddressPrimary = new AddressDetail
                                                             {
                                                                 CountryName = userData.HometownLocation.Country.ToString(),
                                                                 StateName = userData.HometownLocation.State.ToString(),
                                                                 CityName = userData.HometownLocation.City,
                                                                 PostalCode = userData.HometownLocation.ZipCode,
                                                             },

                                PictureName = (pictureBytes == null) ? null : "Facebook.jpg",
                                PictureData = pictureBytes,
                                PersonalProfileIdentifiers = new ProfileIdentifiers { FacebookProfileId = userData.UserId }
                            });
                }
            }

            return resultList;
        }

        /// <summary>
        /// Write access is not implemented.
        /// </summary>
        /// <param name="elements">The parameter elements id not used by this connector.</param>
        /// <param name="clientFolderName">The parameter clientFolderName id not used by this connector.</param>
        /// <param name="skipIfExisting">The parameter skipIfExisting id not used by this connector.</param>
        /// <exception cref="NotImplementedException">
        /// Write access is not implemented.
        /// </exception>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }
    }
}
