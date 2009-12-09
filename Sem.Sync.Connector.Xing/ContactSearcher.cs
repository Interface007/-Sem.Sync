// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactSearcher.cs" company="Sven Erik Matzen">
//   (C) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ContactSearcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Xing
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;

    using SyncBase.DetailData;

    /// <summary>
    /// performs lookups for contacts in Xing based on a list of contacts stored inside the memory connector
    /// </summary>
    public class ContactSearcher : StdClient
    {
        /// <summary>
        /// Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "https://www.xing.com";

        private HttpHelper xingRequester;

        public ContactSearcher()
        {
            this.xingRequester = new HttpHelper(HttpUrlBaseAddress, true)
            {
                UseCache = this.GetConfigValueBoolean("UseCache"),
                SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"),
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the memory connector. </param>
        /// <param name="result"> The result list of contacts. </param>
        /// <returns> the search results </returns>
        protected override System.Collections.Generic.List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var connector = new Memory.GenericClient();
            var listToScan = connector.GetAll(clientFolderName);
            this.xingRequester.UiDispatcher = this.UiDispatcher;

            result = new List<StdElement>();

            foreach (StdContact element in listToScan)
            {
                if (!string.IsNullOrEmpty(element.PersonalProfileIdentifiers.XingNameProfileId)
                    || string.IsNullOrEmpty(element.Name.LastName))
                {
                    continue;
                }

                var guesses = this.GuessProfileUrls(element.Name);
                foreach (var guess in guesses)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        var profileUrl = "http://www.xing.com/profile/" + guess + ((i > 0) ? i.ToString() : string.Empty);
                        var publicProfile = this.xingRequester.GetContent(profileUrl);

                        if (publicProfile.Contains("Die gesuchte Seite konnte nicht gefunden werden."))
                        {
                            break;
                        }

                        var imageUrl = this.MapRegexToProperty(
                            publicProfile,
                            @"id=""photo"" src=""(?<info>/img/users/[^""]/[^""]/[^""]*)"" class=""photo profile-photo""");
                        var newContact = new StdContact
                            {
                                Name =
                                    this.MapRegexToProperty(
                                    publicProfile, "\\<meta name=\"author\" content=\"(?<info>[^\"]*)\""),
                                BusinessPosition =
                                    this.MapRegexToProperty(
                                    publicProfile, "\\<p class=\"profile-work-descr\"\\>(\\<[^>]*>)*(?<info>[^<]*)\\</"),
                                BusinessAddressPrimary =
                                    new AddressDetail
                                        {
                                            PostalCode =
                                                this.MapRegexToProperty(
                                                publicProfile, "zip_code=\\%22(?<info>[^%]*)\\%22"),
                                            CityName =
                                                this.MapRegexToProperty(
                                                publicProfile, "search&amp;city=%22(?<info>[^%]*)%22")
                                        },
                                PictureData =
                                    string.IsNullOrEmpty(imageUrl)
                                        ? null
                                        : this.xingRequester.GetContentBinary(
                                              this.MapRegexToProperty(
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

        private List<string> GuessProfileUrls(PersonName name)
        {
            var result = new List<string>
                {
                    name.FirstName + "_" + name.LastName
                };

            if (!string.IsNullOrEmpty(name.MiddleName))
            {
                result.Add(name.FirstName + name.MiddleName.Replace(".", string.Empty).Replace("-", string.Empty) + "_" + name.LastName);
            }

            return result;
        }

        private string MapRegexToProperty(string profile, string regEx)
        {
            var information = Regex.Matches(profile, regEx, RegexOptions.Singleline);
            return 
                information.Count > 0 
                ? information[0].Groups["info"].ToString() 
                : string.Empty;
        }
    }
}
