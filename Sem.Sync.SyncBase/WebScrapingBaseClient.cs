// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingBaseClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the base client class for handling contacts using web scraping technology.
//   It implements handling the basics of login, download of friend list and contacts content.
//   The inherited class needs to provifr information about the web site via properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// This class is the base client class for handling contacts using web scraping technology.
    ///   It implements handling the basics of login, download of friend list and contacts content.
    ///   The inherited class needs to provifr information about the web site via properties.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true, ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "WebScraping-Base-Client", Internal = true, CanReadContacts = false, 
        CanWriteContacts = false, NeedsCredentials = true, MatchingIdentifier = ProfileIdentifierType.Default)]
    public abstract class WebScrapingBaseClient : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        ///   This parametrized constructore does accept a "ready to use" http-requester. 
        ///   This way you can specify a requester with properties that differ from 
        ///   default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">
        /// the preconfigured http-helper class
        /// </param>
        protected WebScrapingBaseClient(HttpHelper preconfiguredHttpHelper)
        {
            this.httpRequester = preconfiguredHttpHelper;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        ///   The default constructor will create and configure a new http-requester by reading 
        ///   the config file. Use the parametrized constructor to provide a "ready-to-use"
        ///   http-requester.
        /// </summary>
        /// <param name="httpUrlBaseAddress">
        /// The http Base Address of this connector. 
        /// </param>
        protected WebScrapingBaseClient(string httpUrlBaseAddress)
        {
            this.httpRequester = new HttpHelper(httpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"), 
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"), 
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"), 
                };
        }

        #endregion

        #region Properties

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
        ///   Gets the WebSideParameters which defines how to deal with the site.
        /// </summary>
        protected abstract WebSideParameters WebSideParameters { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl">
        /// The contact url. 
        /// </param>
        /// <param name="content">
        /// The content. 
        /// </param>
        /// <returns>
        /// a new StdClient created from the data provided 
        /// </returns>
        protected abstract StdContact ConvertToStdContact(string contactUrl, string content);

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
            this.httpRequester.UiDispatcher = this.UiDispatcher;
            var contactUrls = this.GetUrlList();

            foreach (var contactUrl in contactUrls)
            {
                result.Add(
                    this.DownloadContact(
                        string.Format(
                            CultureInfo.InvariantCulture, this.WebSideParameters.HttpUrlContactDownload, contactUrl)));
            }

            result.Sort();
            return result;
        }

        /// <summary>
        /// Convert contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl">
        /// The contact url. 
        /// </param>
        /// <returns>
        /// the downloaded information inserted into a <see cref="StdContact"/> 
        /// </returns>
        private StdContact DownloadContact(string contactUrl)
        {
            var content = this.httpRequester.GetContent(contactUrl, contactUrl, string.Empty);
            var result = this.ConvertToStdContact(contactUrl, content);
            if (!string.IsNullOrEmpty(this.WebSideParameters.ExtractorProfilePictureUrl))
            {
                var pictureUrl = Regex.Match(content, this.WebSideParameters.ExtractorProfilePictureUrl);
                if (pictureUrl.Groups.Count > 1)
                {
                    var pictureUrlString = pictureUrl.Groups[1].ToString();
                    if (
                        !pictureUrlString.EndsWith(
                            this.WebSideParameters.ImagePlaceholderUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        result.PictureData = this.httpRequester.GetContentBinary(
                            pictureUrlString, contactUrl, string.Empty);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <returns>
        /// a list of urls for the vCards to be downloaded
        /// </returns>
        private List<string> GetUrlList()
        {
            var result = new List<string>();

            while (true)
            {
                this.httpRequester.LogOnFormDetectionString = this.WebSideParameters.HttpDetectionStringLogOnNeeded;

                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                var theContact =
                    this.httpRequester.GetContent(
                        string.Format(CultureInfo.InvariantCulture, this.WebSideParameters.HttpUrlFriendList, 0), 
                        "HttpUrlFriendList");
                var friendIds = Regex.Match(theContact, this.WebSideParameters.ExtractorFriendUrls);

                if (friendIds.Groups.Count >= 2)
                {
                    foreach (var capture in friendIds.Groups["id"].Captures)
                    {
                        result.Add(capture.ToString());
                    }

                    return result;
                }

                bool logonFailed = !GetLogon();

                if (logonFailed)
                {
                    return result;
                }
            }
        }

        public bool GetLogon()
        {
            if (string.IsNullOrEmpty(this.LogOnPassword))
            {
                this.QueryForLogOnCredentials("needs some credentials");
            }

            // prepare the post data for log on
            var postData = HttpHelper.PreparePostData(
                this.WebSideParameters.HttpDataLogOnRequest, this.LogOnUserId, this.LogOnPassword);

            // post to get the cookies
            var logInResponse = this.httpRequester.GetContentPost(
                this.WebSideParameters.HttpUrlLogOnRequest, "logOn", postData);
            return !logInResponse.Contains(this.WebSideParameters.HttpDetectionStringLogOnFailed);
        }

        #endregion
    }
}