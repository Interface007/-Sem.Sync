// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactSearcher.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   performs lookups for contacts in Xing based on a list of contacts stored inside the memory connector
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Xing
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.Memory;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// performs lookups for contacts in Xing based on a list of contacts stored inside the memory connector
    /// </summary>
    public class ContactSearcher : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "https://www.xing.com";

        /// <summary>
        ///   The http-requester for this instance
        /// </summary>
        private readonly HttpHelper xingRequester;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ContactSearcher" /> class.
        /// </summary>
        public ContactSearcher()
        {
            this.xingRequester = new HttpHelper(HttpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"), 
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"), 
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"), 
                };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implements a read of the full list of contacts for the specifies folder.
        /// </summary>
        /// <param name="clientFolderName">
        /// The client folder name for the memory connector. 
        /// </param>
        /// <param name="result">
        /// The result list of contacts. 
        /// </param>
        /// <returns>
        /// the search results 
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var connector = new GenericClient();
            var listToScan = connector.GetAll(clientFolderName);
            this.xingRequester.UiDispatcher = this.UiDispatcher;

            result = new List<StdElement>();

            foreach (StdContact element in listToScan)
            {
                if (
                    !string.IsNullOrEmpty(
                        element.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId)) ||
                    string.IsNullOrEmpty(element.Name.LastName))
                {
                    continue;
                }

                var guesses = GuessProfileUrls(element.Name);
                foreach (var guess in guesses)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        var profileUrl = "http://www.xing.com/profile/" + guess +
                                         ((i > 0) ? i.ToString(CultureInfo.InvariantCulture) : string.Empty);
                        var publicProfile = this.xingRequester.GetContent(profileUrl);

                        if (publicProfile.Contains("Die gesuchte Seite konnte nicht gefunden werden."))
                        {
                            break;
                        }

                        var imageUrl = MapRegexToProperty(
                            publicProfile, 
                            @"id=""photo"" src=""(?<info>/img/users/[^""]/[^""]/[^""]*)"" class=""photo profile-photo""");
                        var newContact = new StdContact
                            {
                                Name =
                                    MapRegexToProperty(
                                        publicProfile, "\\<meta name=\"author\" content=\"(?<info>[^\"]*)\""), 
                                BusinessPosition =
                                    MapRegexToProperty(
                                        publicProfile, 
                                        "\\<p class=\"profile-work-descr\"\\>(\\<[^>]*>)*(?<info>[^<]*)\\</"), 
                                BusinessAddressPrimary =
                                    new AddressDetail
                                        {
                                            PostalCode =
                                                MapRegexToProperty(publicProfile, "zip_code=\\%22(?<info>[^%]*)\\%22"), 
                                            CityName =
                                                MapRegexToProperty(
                                                    publicProfile, "search&amp;city=%22(?<info>[^%]*)%22")
                                        }, 
                                PictureData =
                                    string.IsNullOrEmpty(imageUrl)
                                        ? null
                                        : this.xingRequester.GetContentBinary(
                                            MapRegexToProperty(
                                                publicProfile, 
                                                @"id=""photo"" src=""(?<info>/img/users/[^""]/[^""]/[^""]*)"" class=""photo profile-photo"""))
                            };

                        if (string.IsNullOrEmpty(newContact.Name.ToString()))
                        {
                            continue;
                        }

                        this.LogProcessingEvent(newContact, "adding new contact candidate");

                        result.Add(newContact);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a list of possible user profile urls for Xing.
        /// </summary>
        /// <param name="name">
        /// The name of the person to generate the urls. 
        /// </param>
        /// <returns>
        /// The list of urls. 
        /// </returns>
        private static List<string> GuessProfileUrls(PersonName name)
        {
            var result = new List<string> { name.FirstName + "_" + name.LastName };

            if (!string.IsNullOrEmpty(name.MiddleName))
            {
                result.Add(
                    name.FirstName + name.MiddleName.Replace(".", string.Empty).Replace("-", string.Empty) + "_" +
                    name.LastName);
            }

            return result;
        }

        /// <summary>
        /// Extracts information from a profile.
        /// </summary>
        /// <param name="profile">
        /// The profile content. 
        /// </param>
        /// <param name="regEx">
        /// The regex to get the information. 
        /// </param>
        /// <returns>
        /// the information as a string
        /// </returns>
        private static string MapRegexToProperty(string profile, string regEx)
        {
            var information = Regex.Matches(profile, regEx, RegexOptions.Singleline);
            return information.Count > 0 ? information[0].Groups["info"].ToString() : string.Empty;
        }

        #endregion
    }
}