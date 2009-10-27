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
    [ConnectorDescription(
        DisplayName = "WebScraping-Base-Client",
        Internal = true,
        CanReadContacts = false,
        CanWriteContacts = false,
        NeedsCredentials = true,
        MatchingIdentifier = ProfileIdentifierType.Default)]
    public abstract class WebScrapingBaseClient : StdClient
    {
        /// <summary>
        /// http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

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
        /// <param name="httpUrlBaseAddress"> The http Base Address of this connector. </param>
        protected WebScrapingBaseClient(string httpUrlBaseAddress)
        {
            this.HttpUrlBaseAddress = httpUrlBaseAddress;
            this.httpRequester = new HttpHelper(this.HttpUrlBaseAddress, true)
            {
                UseCache = this.GetConfigValueBoolean("UseCache"),
                SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"),
            };
        }

        /// <summary>
        /// Gets the deterministin part of the placeholder url - the url to a placeholder must match this regex, while
        /// all others must not.
        /// </summary>
        protected abstract string ImagePlaceholderUrl { get; }

        /// <summary>
        /// Gets the url pointing to the contact data to be downloaded - contains a single parameter {0} with the ID from the 
        /// profile ids for the contact to be processed.
        /// </summary>
        protected abstract string HttpUrlContactDownload { get; }

        /// <summary>
        /// Gets the <see cref="ProfileIdentifierType"/> of this source.
        /// </summary>
        protected abstract ProfileIdentifierType ProfileIdentifierType { get; }
        
        /// <summary>
        /// Gets the HttpRequester.
        /// </summary>
        protected HttpHelper HttpRequester
        {
            get { return this.httpRequester; }
        }

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
        /// Gets the regex to extract the picture url from the profile content
        /// </summary>
        protected abstract string ExtractorProfilePictureUrl { get; }

        /// <summary>
        /// Gets the detection string to detect if we did fail to logon
        /// </summary>
        protected abstract string HttpDetectionStringLogOnFailed { get; }

        /// <summary>
        /// Gets the base address to communicate with the site
        /// </summary>
        protected string HttpUrlBaseAddress { get; private set; }

        /// <summary>
        /// Gets the base address to communicate with the site
        /// </summary>
        protected abstract string HttpUrlFriendList { get; }

        /// <summary>
        /// Gets the relative url to log on
        /// </summary>
        protected abstract string HttpUrlLogOnRequest { get; }

        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <param name="content"> The content. </param>
        /// <returns> a new StdClient created from the data provided </returns>
        protected abstract StdContact ConvertToStdContact(string contactUrl, string content);
        
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
                result.Add(this.DownloadContact(string.Format(CultureInfo.InvariantCulture, this.HttpUrlContactDownload, contactUrl)));
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
        /// Convert contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <returns> the downloaded information inserted into a <see cref="StdContact"/> </returns>
        private StdContact DownloadContact(string contactUrl)
        {
            var content = this.httpRequester.GetContent(contactUrl, contactUrl, string.Empty);
            var result = this.ConvertToStdContact(contactUrl, content);
            if (!string.IsNullOrEmpty(this.ExtractorProfilePictureUrl))
            {
                var pictureUrl = Regex.Match(content, this.ExtractorProfilePictureUrl);
                if (pictureUrl.Groups.Count > 1)
                {
                    var pictureUrlString = pictureUrl.Groups[1].ToString();
                    if (!pictureUrlString.EndsWith(this.ImagePlaceholderUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        result.PictureData = this.httpRequester.GetContentBinary(pictureUrlString, contactUrl, string.Empty);
                    }
                }
            }

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
                this.httpRequester.LogOnFormDetectionString = this.HttpDetectionStringLogOnNeeded;

                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                var theContact = this.httpRequester.GetContent(string.Format(CultureInfo.InvariantCulture, this.HttpUrlFriendList, 0), "HttpUrlFriendList");
                var friendIds = Regex.Match(theContact, this.ExtractorFriendUrls);

                if (friendIds.Groups.Count >= 2)
                {
                    foreach (var capture in friendIds.Groups["id"].Captures)
                    {
                        result.Add(capture.ToString());
                    }

                    return result;
                }

                if (string.IsNullOrEmpty(this.LogOnPassword))
                {
                    QueryForLogOnCredentials("needs some credentials");
                }

                // prepare the post data for log on
                var postData = HttpHelper.PreparePostData(
                    this.HttpDataLogOnRequest,
                    this.LogOnUserId,
                    this.LogOnPassword);

                // post to get the cookies
                var logInResponse = this.httpRequester.GetContentPost(this.HttpUrlLogOnRequest, "logOn", postData);
                if (logInResponse.Contains(this.HttpDetectionStringLogOnFailed))
                {
                    return result;
                }
            }
        }
    }
}
