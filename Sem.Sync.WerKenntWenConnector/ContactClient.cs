﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.WerKenntWenConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.DetailData;
    
    #endregion usings

    /// <summary>
    /// Client implementation for reading information from www.wer-kennt-wen.de
    /// </summary>
    public class ContactClient : StdClient
    {
        #region string resources for processing wkw pages

        /// <summary>
        /// Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "id=\"loginform\"";

        /// <summary>
        /// detection string to detect if we did fail to logon
        /// </summary>
        private const string HttpDetectionStringLogonFailed = "/app/user?op=lostpassword";

        /// <summary>
        /// Base address to communicate with Wer-Kennt-Wen
        /// </summary>
        private const string HttpUrlBaseAddress = "http://www.wer-kennt-wen.de";

        /// <summary>
        /// relative url to log on
        /// </summary>
        private const string HttpUrlLogonRequest = "/start.php";

        /// <summary>
        /// relative URL to query contact links to personal pages
        /// </summary>
        private const string HttpUrlListContent = "/people/friends";

        /// <summary>
        /// data string to be posted to logon into wer-kennt-wen
        /// </summary>
        private const string HttpDataLogonRequest = "loginName={0}&pass={1}&x=0&y=0&logIn=1";

        /// <summary>
        /// regular expression to extract the URLs for the personal pages
        /// </summary>
        private const string PatternGetDataUrls = ".div class=\"pl-pic\".*?a href=\"([a-zA-Z0-9/]+)\"";

        #endregion

        #region private fields

        /// <summary>
        /// http requester object that will read the data from xing
        /// </summary>
        private readonly HttpHelper wkwRequester;

        #endregion

        /// <summary>
        /// cache hint string constant to specify a daily refresh for the cached files
        /// </summary>
        private const string CacheHintRefresh = "[REFRESH=DAILY]";

        /// <summary>
        /// cache hint string constant to specify that this item should not be cached at all
        /// </summary>
        private const string CacheHintNoCache = "[NOCACHE]";

        /// <summary>
        /// Extracts the data from the page
        /// </summary>
        private const string PersonDataExtractionPattern = "/users/([a-zA-Z0-9 %\\+]*)/([a-zA-Z0-9 %\\+-]*)";

        /// <summary>
        /// Extracts the url of the picture from the page
        /// </summary>
        private const string PersonPictureUrlPattern = "div id=\"person_picture\".*?img src=\"(.*?)\"";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
        {
            this.wkwRequester = new HttpHelper(HttpUrlBaseAddress, true)
            {
                UseCache = false,
                SkipNotCached = false,
                UseIeCookies = false,
            };
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Wer-Kennt-Wen";
            }
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var wkwContacts = this.GetUrlList();

            foreach (var item in wkwContacts)
            {
                var contact = this.DownloadContact(item, item.Replace("/", "_").Replace("?", "_"));
                if (contact != null)
                {
                    result.Add(contact);
                }
            }

            return result;
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ready a list of html locations of the person data - this will also establish the logon
        /// </summary>
        /// <returns>a list of urls for the data to be downloaded</returns>
        private List<string> GetUrlList()
        {
            var result = new List<string>();

            string contactListContent;
            
            while (true)
                {
                    // optimistically we try to read the content without explicit logon
                    // this will succeed if we have a valid cookie
                    contactListContent = this.wkwRequester.GetContent(
                        string.Format(CultureInfo.InvariantCulture, HttpUrlListContent),
                        "UrlList" + CacheHintRefresh);

                    // if we don't find the logon form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLogonNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        QueryForLogOnCredentials("Wer kennt wen benötigt die Log-In-Daten.");
                    }

                    // tell the user that we need to log on
                    LogProcessingEvent("Wer kennt wen benötigt die Log-In-Daten.", this.LogOnUserId);

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLogonRequest,
                        this.LogOnUserId,
                        this.LogOnPassword);

                    // post to get the cookies
                    var logInResponse = this.wkwRequester.GetContentPost(HttpUrlLogonRequest, CacheHintNoCache, postData);

                    if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                    {
                        LogProcessingEvent("Log-In ist fehlgeschlagen.", this.LogOnUserId);
                        return result;
                    }

                    // we did succeed to log on - tell the user and try reading the data again.
                    LogProcessingEvent("Login erfolgreich", this.LogOnUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var urlExtractor = new Regex(PatternGetDataUrls, RegexOptions.Singleline);
                var matches = urlExtractor.Matches(contactListContent);

                LogProcessingEvent("füge Kontakte hinzu...", matches.Count, result.Count);

                // add the matches to the result
                foreach (Match match in matches)
                {
                    result.Add(match.Groups[1].ToString());
                }

            return result;
        }

        /// <summary>
        /// downloads a contact data from wkw and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">url to the wkw page</param>
        /// <param name="name">name that will be used for the cache to store the data for later review</param>
        /// <returns>the downloaded contact as a StdContact</returns>
        private StdContact DownloadContact(string downloadUrl, string name)
        {
            var data = this.wkwRequester.GetContent(downloadUrl, name + ".txt");
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            // we use regular expressions to extract the urls to the vCards
            var dataExtractor = new Regex(PersonDataExtractionPattern, RegexOptions.Singleline);
            var matches = dataExtractor.Matches(data);

            var contact = new StdContact
                {
                    Id = Guid.NewGuid(),
                    PersonalAddressPrimary = new AddressDetail(), 
                    Name = new PersonName(),
                };

            var birthday = string.Empty;
            var birthyear = string.Empty;

            foreach (Match match in matches)
            {
                var key = match.Groups[1].ToString();
                
                // the encoding needs to be set to get the correct special chars
                var value = HttpUtility.UrlDecode(match.Groups[2].ToString(), Encoding.GetEncoding("iso8859-1"));
                switch (key)
                {
                    case "":
                        break;

                    // The following information is provided by Wer-kennt-wen, but not handled by sem.sync, yet.
                    // It may be handled with one of the next versions.
                    case "hobbies":
                    case "music":
                    case "movies":
                    case "placesToVisit":
                    case "placesVisited":
                    case "books":
                    case "jobclass":
                    case "partnership":
                        break;

                    case "firstName":
                        contact.Name.FirstName = value;
                        break;
                    
                    case "lastName":
                        contact.Name.LastName = value;
                        break;
                    
                    case "city":
                        contact.PersonalAddressPrimary.CityName = value;
                        break;
                    
                    case "zipCode":
                        contact.PersonalAddressPrimary.PostalCode = value;
                        break;
                    
                    case "gender":
                        contact.PersonGender = match.Groups[2].ToString() == "1" ? Gender.Female : Gender.Male;
                        break;

                    case "birthyear":
                        birthyear = value;
                        break;

                    case "birthday":
                        birthday = value;
                        break;

                    default:
                        Console.WriteLine("unknown attribute: " + key + " - " + value);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(birthyear) && !string.IsNullOrEmpty(birthday))
            {
                if (birthday.Length == 2)
                {
                    birthday = birthday + "-01";
                }

                contact.DateOfBirth = new DateTime(int.Parse(birthyear.Substring(0, 4)), int.Parse(birthday.Substring(0, 2)), int.Parse(birthday.Substring(3, 2)));
            }

            contact.PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.WerKenntWenUrl, downloadUrl);

            dataExtractor = new Regex(PersonPictureUrlPattern, RegexOptions.Singleline);
            matches = dataExtractor.Matches(data);

            if (matches.Count == 1)
            {
                var pictureName = matches[0].Groups[1].ToString();
                if (!pictureName.Contains("images/dummy"))
                {
                    contact.PictureName = pictureName.Substring(pictureName.LastIndexOf('/') + 1);
                    contact.PictureData = this.wkwRequester.GetContentBinary(pictureName, pictureName);
                }
            }

            LogProcessingEvent(contact, "downloaded");
            return contact;
        }
    }
}
