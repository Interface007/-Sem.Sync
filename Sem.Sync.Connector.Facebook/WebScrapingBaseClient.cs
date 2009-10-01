// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingBaseClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the base client class for handling contacts using web scraping
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the base client class for handling contacts using web scraping technology
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    //// specifying the connector description as no-read and no-write will hide it from the GUI
    [ConnectorDescription(DisplayName = "WebScraping-Base-Client",
        CanRead = false,
        CanWrite = false,
        MatchingIdentifier = ProfileIdentifierType.Default,
        NeedsCredentials = true)]
    public abstract class WebScrapingBaseClient : StdClient
    {
        /// <summary>
        /// http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        /// This parametrized constructore does accept a "ready to use" http-requester. 
        /// This way you can specify a requester with properties that differ from 
        /// default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">the preconfigured http-helper class</param>
        protected WebScrapingBaseClient(HttpHelper preconfiguredHttpHelper)
        {
            this.httpRequester = preconfiguredHttpHelper;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        /// The default constructor will create and configure a new http-requester by reading 
        /// the config file. Use the parametrized constructor to provide a "ready-to-use"
        /// http-requester.
        /// </summary>
        protected WebScrapingBaseClient()
        {
            this.HttpDetectionStringLogOnFailed = "action=\"https://secure.meinvz.net/Login\"";
            this.HttpUrlLogOnRequest = "https://secure.meinvz.net/Login";
            this.HttpUrlBaseAddress = "http://www.meinvz.net";
            this.ContactImageSelector = "src=\"(http://[-a-z0-9.]*imagevz.net/profile[-/a-z0-9]*.jpg)\" class=\"obj-profileImage\" id=\"profileImage\"";

            this.httpRequester = new HttpHelper(this.HttpUrlBaseAddress, true)
            {
                UseCache = this.GetConfigValueBoolean("UseCache"),
                SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"),
            };
        }

        #endregion

        #region string resources for processing the pages

        /// <summary>
        /// Gets the detection string to parse the content of a request if we need to logon
        /// </summary>
        protected abstract string HttpDetectionStringLogOnNeeded { get; }

        /// <summary>
        /// Gets the data string to be posted to logon into the site
        /// </summary>
        protected abstract string HttpDataLogOnRequest { get; }

        /// <summary>
        /// Gets the regex to extract the form key for the log on
        /// </summary>
        protected abstract string ExtractorFormKey { get; }

        /// <summary>
        /// Gets the regex to extract the iv for the log on
        /// </summary>
        protected abstract string ExtractorIv { get; }

        /// <summary>
        /// Gets the regex to extract the iv for the log on
        /// </summary>
        protected abstract string ExtractorFriendUrls { get; }

        /// <summary>
        /// Gets the regex to extract additional information
        /// </summary>
        protected abstract string ContactContentSelector { get; }

        #endregion
        
        /// <summary>
        /// Gets the HttpRequester.
        /// </summary>
        protected HttpHelper HttpRequester
        {
            get
            {
                return this.httpRequester;
            }
        }

        /// <summary>
        /// Gets or sets the extraction string for the image.
        /// </summary>
        protected string ContactImageSelector { get; set; }

        /// <summary>
        /// Gets or sets the detection string to detect if we did fail to logon
        /// </summary>
        protected string HttpDetectionStringLogOnFailed { get; set; }

        /// <summary>
        /// Gets or sets the base address to communicate with the site
        /// </summary>
        protected string HttpUrlBaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the relative url to log on
        /// </summary>
        protected string HttpUrlLogOnRequest { get; set; }

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
            this.httpRequester.UiDispatcher = this.UiDispatcher;
            var contactUrls = this.GetUrlList();

            foreach (var contactUrl in contactUrls)
            {
                result.Add(this.DownloadContact(contactUrl));
            }

            result.Sort();
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
        /// Convert MeinVZ contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <returns> the downloaded information inserted into a <see cref="StdContact"/> </returns>
        private StdContact DownloadContact(string contactUrl)
        {
            var result = new StdContact();

            var content = this.httpRequester.GetContent(contactUrl, contactUrl, string.Empty);
            var imageUrl = Regex.Match(content, this.ContactImageSelector, RegexOptions.Singleline);
            if (imageUrl != null)
            {
                var url = imageUrl.Groups[1].ToString();
                result.PictureName = url.Substring(url.LastIndexOf('/') + 1);
                result.PictureData = this.httpRequester.GetContentBinary(url, url);
            }

            foreach (Match match in Regex.Matches(content, this.ContactContentSelector, RegexOptions.Singleline))
            {
                var key = match.Groups[1].ToString();
                var value = match.Groups[2].ToString();
                if (value.Contains(">"))
                {
                    value = value.Substring(value.IndexOf('>') + 1);
                }

                if (value.Contains("<"))
                {
                    value = value.Substring(0, value.IndexOf('<'));
                }

                result.InternalSyncData = new SyncData();

                switch (key)
                {
                    case "Name:":
                        result.Name = new PersonName(value);
                        break;

                    case "Mitglied seit:":
                        result.InternalSyncData.DateOfCreation = DateTime.Parse(value, CultureInfo.CurrentCulture);
                        break;

                    case "Letztes Update:":
                        result.InternalSyncData.DateOfLastChange = DateTime.Parse(value, CultureInfo.CurrentCulture);
                        break;

                    case "Geschlecht:":
                        result.PersonGender = value == "männlich" ? Gender.Male : Gender.Female;
                        break;

                    case "Geburtstag:":
                        result.DateOfBirth = DateTime.Parse(value, CultureInfo.CurrentCulture);
                        break;

                    case "Skype:":
                        result.PersonalInstantMessengerAddresses = result.PersonalInstantMessengerAddresses ?? new InstantMessengerAddresses();
                        result.PersonalInstantMessengerAddresses.Skype = value.Replace("&nbsp;", " ");
                        break;

                    case "Handy:":
                        result.PersonalPhoneMobile = new PhoneNumber(value);
                        break;

                    case "Telefon:":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        result.PersonalAddressPrimary.Phone = new PhoneNumber(value);
                        break;

                    case "Anschrift:":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        result.PersonalAddressPrimary.StreetName = value;
                        break;

                    case "Ort:":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        while (value.Contains("  "))
                        {
                            value = value.Replace("  ", " ");
                        }

                        if (Regex.IsMatch(value, "^[0-9]+ "))
                        {
                            result.PersonalAddressPrimary.PostalCode = value.Split(' ')[0];
                            result.PersonalAddressPrimary.CityName = value.Split(' ')[1];
                        }
                        else
                        {
                            result.PersonalAddressPrimary.CityName = value;
                        }

                        break;

                    case "Land:":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        result.PersonalAddressPrimary.CountryName = value;
                        break;

                    case "Webseite:":
                        result.PersonalHomepage = value;
                        break;

                    case "Auf der Suche nach:":
                        break;

                    case "Firma:":
                        result.BusinessCompanyName = value;
                        break;

                    case "Position:":
                        result.BusinessPosition = value;
                        break;

                    default:
                        Console.WriteLine("new content: " + key + " => " + value);
                        break;
                }
            }

            result.PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.MeinVZ, contactUrl.Substring(contactUrl.LastIndexOf("/", StringComparison.Ordinal) + 1));

            return result;
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <returns>a list of urls for the vCards to be downloaded</returns>
        private List<string> GetUrlList()
        {
            var result = new List<string>();

            while (true)
            {
                List<string> extractedData;
                this.httpRequester.LogOnFormDetectionString = this.HttpDetectionStringLogOnNeeded;

                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                if (this.httpRequester.GetExtract(string.Empty, this.ExtractorFriendUrls, out extractedData, "FriendUrls", string.Empty))
                {
                    if (this.httpRequester.GetExtract(extractedData[0], "<a href=\"(/Profile/[0-9a-z]*)\"", out result))
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(this.LogOnPassword))
                {
                    QueryForLogOnCredentials("needs some credentials");
                }

                var matches = Regex.Matches(this.httpRequester.LastExtractContent, this.ExtractorFormKey, RegexOptions.Singleline);
                var formKey = matches[0].Groups[1].Captures[0].ToString();

                matches = Regex.Matches(this.httpRequester.LastExtractContent, this.ExtractorIv, RegexOptions.Singleline);
                var iv = matches[0].Groups[1].Captures[0].ToString();

                // prepare the post data for log on
                var postData = HttpHelper.PreparePostData(
                    this.HttpDataLogOnRequest,
                    this.LogOnUserId,
                    this.LogOnPassword,
                    formKey,
                    iv);

                // post to get the cookies
                var logInResponse = this.httpRequester.GetContentPost(this.HttpUrlLogOnRequest, "logOn", postData);

                if (logInResponse.Contains(this.HttpDetectionStringLogOnFailed))
                {
                    return result;
                }
            }

            return result;
        }
    }
}
