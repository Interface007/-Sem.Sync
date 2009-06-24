﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.FacebookConnector
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Win32;

    using Facebook.Components;

    using SyncBase;
    using SyncBase.DetailData;

    /// <summary>
    /// Implements a reading client for the Facebook api. Unfortunately Facebook is not
    /// publishing too much data about friends.
    /// </summary>
    public class ContactClient : StdClient
    {
        private readonly string apiKey;
        private readonly string apiSecret;

        /// <summary>
        /// Creates a new instance of the <see cref="ContactClient"/> class and initializes the apiKey 
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

        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var resultList = new List<StdElement>();
            var service = new FacebookService { ApplicationKey = apiKey, Secret = apiSecret };

            service.ConnectToFacebook();
            var friendList = service.GetFriendIds();
            foreach (var friend in friendList)
            {
                Facebook.Entity.User userData = null;
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
                                    (userData.Sex == Facebook.Entity.Gender.Female)
                                        ? Gender.Female
                                        :
                                            (userData.Sex == Facebook.Entity.Gender.Male)
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
                                                                 CountryName =
                                                                     userData.HometownLocation.Country.ToString(""),
                                                                 StateName =
                                                                     userData.HometownLocation.State.ToString(""),
                                                                 CityName = userData.HometownLocation.City,
                                                                 PostalCode = userData.HometownLocation.ZipCode,
                                                             },

                                PictureName = (pictureBytes == null) ? null : "Facebook.jpg",
                                PictureData = pictureBytes,
                                PersonalProfileIdentifiers = new ProfileIdentifiers { FacebookProfileId = userData.UserId }
                            }
                        );
                }
            }

            return resultList;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }

        public override sealed string FriendlyClientName
        {
            get { return "Facebook-Connector"; }
        }
    }
}
