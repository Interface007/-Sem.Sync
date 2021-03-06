﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client base class for handling contacts of the MeinVZ/StudiVZ social network.
//   See the classes <see cref="MeinVZContacts" /> and <see cref="StudiVzContacts" /> for concrete
//   implementation of the <see cref="StdClient" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MeinVZ
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client base class for handling contacts of the MeinVZ/StudiVZ social network.
    ///   See the classes <see cref="MeinVzContacts"/> and <see cref="StudiVzContacts"/> for concrete 
    ///   implementation of the <see cref="StdClient"/>.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true, ReferenceType = ClientPathType.Undefined)]
    //// specifying the connector description as no-read and no-write will hide it from the GUI
    [ConnectorDescription(DisplayName = "MeinVZ-Base-Client", CanReadContacts = false, CanWriteContacts = false, 
        MatchingIdentifier = ProfileIdentifierType.MeinVZ, NeedsCredentials = true)]
    public abstract class ContactClient : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   regex to extract additional information
        /// </summary>
        private const string ContactContentSelector = "<dt>([a-zA-Z: ]*)</dt>.*?<dd>\\s*(.*?)\\s*</dd>";

        /// <summary>
        ///   regex to extract the form key for the log on
        /// </summary>
        private const string ExtractorFormKey = "<input type=\"hidden\" name=\"formkey\" value=\"([a-zA-Z0-9_-]*)\" />";

        /// <summary>
        ///   regex to extract the url to the "Friends List"
        /// </summary>
        private const string ExtractorFriendUrls = "<a href=\"(/Friends/All/[a-zA-Z0-9_-]*/tid/[0-9]*)\" rel=\"nofollow\"\\s+title=\"Meine Freunde\">Meine Freunde</a>";

        /// <summary>
        ///   regex to extract the iv for the log on
        /// </summary>
        private const string ExtractorIv = "<input type=\"hidden\" name=\"iv\" value=\"([a-zA-Z0-9_-]*)\" />";

        /// <summary>
        ///   regex to extract the urls to the friends profiles
        /// </summary>
        private const string ExtractorProfileUrls = "<a href=\"(/Profile/[a-zA-Z0-9_-]*)\"";

        /// <summary>
        ///   data string to be posted to logon into the site
        /// </summary>
        private const string HttpDataLogonRequest = "email={0}&" + "password={1}&" + "login=Einloggen&jsEnabled=false&" + "formkey={2}&" + "iv={3}";

        /// <summary>
        ///   Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "form id=\"Loginbox\"";

        /// <summary>
        ///   http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        ///   This parametrized constructore does accept a "ready to use" http-requester. 
        ///   This way you can specify a requester with properties that differ from 
        ///   default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">
        /// the preconfigured http-helper class
        /// </param>
        protected ContactClient(HttpHelper preconfiguredHttpHelper)
        {
            this.httpRequester = preconfiguredHttpHelper;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ContactClient" /> class. 
        ///   The default constructor will create and configure a new http-requester by reading 
        ///   the config file. Use the parametrized constructor to provide a "ready-to-use"
        ///   http-requester.
        /// </summary>
        protected ContactClient()
        {
            this.ContactImageSelector = "src=\"(http://[-a-z0-9.]*imagevz.net/profile[-/a-z0-9]*.jpg)\" class=\"obj-profileImage\" id=\"profileImage\"";

            this.httpRequester = new HttpHelper(this.HttpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"), 
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"), 
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"), 
                };
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "MeinVZ-Connector";
            }
        }

        /// <summary>
        ///   Gets or sets the extraction string for the image.
        /// </summary>
        protected string ContactImageSelector { get; set; }

        /// <summary>
        ///   Gets or sets the detection string to detect if we did fail to logon
        /// </summary>
        protected string HttpDetectionStringLogOnFailed { get; set; }

        /// <summary>
        ///   Gets the HttpRequester.
        /// </summary>
        protected HttpHelper HttpRequester
        {
            get
            {
                return this.httpRequester;
            }
        }

        /// <summary>
        ///   Gets or sets the base address to communicate with the site
        /// </summary>
        protected string HttpUrlBaseAddress { get; set; }

        /// <summary>
        ///   Gets or sets the relative url to log on
        /// </summary>
        protected string HttpUrlLogOnRequest { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            this.httpRequester.UiDispatcher = this.UiDispatcher;
            var contactUrls = this.GetUrlList();

            result.AddRange(contactUrls.Select(this.DownloadContact));

            result.Sort();
            return result;
        }

        /// <summary>
        /// Maps a key value pair extracted from the web page to the <see cref="StdContact"/>.
        /// </summary>
        /// <param name="result">
        /// The resulting <see cref="StdContact"/>. 
        /// </param>
        /// <param name="value">
        /// The value part of the pair. 
        /// </param>
        /// <param name="key">
        /// The key part of the pair. 
        /// </param>
        private static void MapKeyValuePair(StdContact result, string value, string key)
        {
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
                    if (value.Length > 9 && Regex.IsMatch(value.Substring(0, 10), @"\d\d\.\d\d\.\d\d\d\d"))
                    {
                        DateTime date;
                        if (DateTime.TryParse(value.Substring(0, 10), CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                        {
                            result.DateOfBirth = date;
                        }
                    }

                    break;

                case "Skype:":
                    result.PersonalInstantMessengerAddresses = result.PersonalInstantMessengerAddresses ??
                                                               new InstantMessengerAddresses();
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
                    Tools.DebugWriteLine("new content: " + key + " => " + value);
                    break;
            }
        }

        /// <summary>
        /// Convert MeinVZ contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl">
        /// The contact url. 
        /// </param>
        /// <returns>
        /// the downloaded information inserted into a <see cref="StdContact"/> 
        /// </returns>
        private StdContact DownloadContact(string contactUrl)
        {
            var result = new StdContact();

            var content = this.httpRequester.GetContent(contactUrl, contactUrl, string.Empty);
            var imageUrl = Regex.Match(content, this.ContactImageSelector, RegexOptions.Singleline);
            if (imageUrl != null)
            {
                var url = imageUrl.Groups[1].ToString();
                if (!string.IsNullOrEmpty(url))
                {
                    result.PictureName = url.Substring(url.LastIndexOf('/') + 1);
                    result.PictureData = this.httpRequester.GetContentBinary(url, url);
                }
            }

            foreach (Match match in Regex.Matches(content, ContactContentSelector, RegexOptions.Singleline))
            {
                var key = match.Groups[1].ToString();
                var value = match.Groups[2].ToString();
                try
                {
                    if (key != "Geburtstag:" && value.Contains(">"))
                    {
                        value = value.Substring(value.IndexOf('>') + 1);
                    }

                    if (value.Contains("<"))
                    {
                        value = value.Substring(0, value.IndexOf('<'));
                    }

                    MapKeyValuePair(result, value, key);
                }
                catch (Exception ex)
                {
                    throw new TechnicalException(
                        "Problem mapping key value pair from web page.", 
                        ex, 
                        new KeyValuePair<string, object>("key", key), 
                        new KeyValuePair<string, object>("value", value));
                }
            }

            result.ExternalIdentifier.SetProfileId(
                ProfileIdentifierType.MeinVZ, 
                contactUrl.Substring(contactUrl.LastIndexOf("/", StringComparison.Ordinal) + 1));

            this.ThinkTime(1000);

            this.LogProcessingEvent(result, "downloaded");

            return result;
        }

        /// <summary>
        /// Ready a list of data locations - this will also establish the logon
        /// </summary>
        /// <returns>
        /// a list of urls for the data to be downloaded
        /// </returns>
        private IEnumerable<string> GetUrlList()
        {
            var result = new List<string>();

            while (true)
            {
                List<string> extractedData;
                this.httpRequester.LogOnFormDetectionString = new[] { HttpDetectionStringLogonNeeded };

                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                if (this.httpRequester.GetExtract(
                    string.Empty, ExtractorFriendUrls, out extractedData, "FriendUrls", string.Empty))
                {
                    if (extractedData.Count > 0 &&
                        this.httpRequester.GetExtract(extractedData[0], ExtractorProfileUrls, out result))
                    {
                        var n = 2;
                        while (true)
                        {
                            List<string> result2;
                            this.httpRequester.GetExtract(
                                extractedData[0].Replace("/tid/103", "/p/" + n), ExtractorProfileUrls, out result2);
                            if (result2.Count == 0)
                            {
                                break;
                            }

                            result.AddRange(result2);
                            n++;
                        }

                        break;
                    }
                }

                if (string.IsNullOrEmpty(this.LogOnPassword))
                {
                    this.QueryForLogOnCredentials("needs some credentials");
                }

                var logInResponse = string.Empty;

                try
                {
                    var matches = Regex.Matches(
                        this.httpRequester.LastExtractContent, ExtractorFormKey, RegexOptions.Singleline);
                    var formKey = matches[0].Groups[1].Captures[0].ToString();

                    matches = Regex.Matches(this.httpRequester.LastExtractContent, ExtractorIv, RegexOptions.Singleline);
                    var iv = matches[0].Groups[1].Captures[0].ToString();

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLogonRequest, this.LogOnUserId, this.LogOnPassword, formKey, iv);

                    // post to get the cookies
                    logInResponse = this.httpRequester.GetContentPost(this.HttpUrlLogOnRequest, "logOn", postData);

                    if (logInResponse.Contains(this.HttpDetectionStringLogOnFailed))
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    throw new TechnicalException(
                        "Problem reading MeinVZ content", 
                        ex, 
                        new KeyValuePair<string, object>("lastcontent", this.httpRequester.LastExtractContent), 
                        new KeyValuePair<string, object>("userid", this.LogOnUserId), 
                        new KeyValuePair<string, object>("loginresponse", logInResponse));
                }
            }

            return result;
        }

        #endregion
    }
}